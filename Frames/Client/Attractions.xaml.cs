using HCI.Models.Attractions.Model;
using HCI.Models.Attractions.Service;
using HCI.Models.Restaurants.Model;
using HCI.Models.Restaurants.Service;
using HCI.Models.Trips.Model;
using HCI.Models.Trips.Service;
using HCI.Models.Users.Model;
using HCI.Tools;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HCI.Frames.Client
{
    /// <summary>
    /// Interaction logic for Attractions.xaml
    /// </summary>
    public partial class Attractions : UserControl
    {
        IAttractionService attractionService;
        IRestaurantService restaurantService;
        IOrderedTripService orderedTripService;
        
        List<Location> locations = new List<Location>();
        OrderedTrip OrderedTrip { get; set; }
        double TotalPriceDouble { get; set; }

        public Attractions(IAttractionService attractionService, IRestaurantService restaurantService,IOrderedTripService  orderedTripService,OrderedTrip trip,User user)
        {
            OrderedTrip = trip;
            this.attractionService = attractionService;
            this.restaurantService = restaurantService;
            this.orderedTripService = orderedTripService;
            InitializeComponent();
            GeocodeAddress(OrderedTrip.Trip.Accommodation.Location.Address);

            var attractions = attractionService.GetAllFromCity(OrderedTrip.Trip.Accommodation.Location.City);
            var restaurants = restaurantService.GetAllFromCity(OrderedTrip.Trip.Accommodation.Location.City);

            var mergedItems = new List<object>();
            mergedItems.AddRange(attractions);
            mergedItems.AddRange(restaurants);
            
            TotalPriceDouble = OrderedTrip.TotalPrice;
            TotalPrice.Text = "Total: " + TotalPriceDouble + "$";



            foreach (var item in attractions)
            {
                var attractionItem = new AttractionItem(item,null, user);
                attractionItem.ItemClicked += AttractionItem_Clicked;
                attractionItemsControl.Items.Add(attractionItem);
            }
            foreach (var item in restaurants)
            {
                var attractionItem = new AttractionItem(null,item, user);
                attractionItem.ItemClicked += AttractionItem_Clicked;
                attractionItemsControl.Items.Add(attractionItem);
            }
        }

        private void AttractionItem_Clicked(object sender, EventArgs e)
        {
            var attractionItem = (AttractionItem)sender;
            if (attractionItem.AttractionData != null)
            {
                if (!OrderedTrip.Attractions.Contains(attractionItem.AttractionData))
                {
                    OrderedTrip.Attractions.Add(attractionItem.AttractionData);
                    addTotalPrice(attractionItem.AttractionData);

                }
                else 
                {
                    OrderedTrip.Attractions.Remove(attractionItem.AttractionData);
                    removeTotalPrice(attractionItem.AttractionData);
                }
                AddPin(attractionItem.AttractionData.Location.Address);

            }
            else
            {

                if (!OrderedTrip.Restaurants.Contains(attractionItem.RestaurantData))
                {
                    OrderedTrip.Restaurants.Add(attractionItem.RestaurantData);
                    
                }
                else
                {
                    OrderedTrip.Restaurants.Remove(attractionItem.RestaurantData);
                }
                AddPin(attractionItem.RestaurantData.Location.Address);

            }

        }

        private void removeTotalPrice(Attraction attractionData)
        {
            TotalPriceDouble -= attractionData.Price;
            TotalPrice.Text = "Total: " + TotalPriceDouble + "$";
        }

        private void addTotalPrice(Attraction attractionData)
        {
            TotalPriceDouble += attractionData.Price;
            TotalPrice.Text = "Total: " + TotalPriceDouble + "$";
        }
        private void OrderClicked(object sender, EventArgs e)
        {
            orderedTripService.AddOrderedTrip(OrderedTrip);
            MessageBox.Show("Order succesfull.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToHistoryClient();
        }

        private async Task AddPin(string address)
        {
            // Replace "YourBingMapsAPIKey" with your actual Bing Maps API key
            string apiKey = "yQf68os4c4HuyVo5IUNR~WX58e8NVpKnTDswqWo0zfQ~AojoZD63rjtEfrLRFwN8lH4sTiqHYWvEAdmIF605L3kS8YTmdUZHHTxnqs72XAf2";
            string url = $"http://dev.virtualearth.net/REST/v1/Locations?query={Uri.EscapeDataString(address)}&key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(jsonResponse);
                    var resourceSets = data.resourceSets;
                    if (resourceSets.Count > 0)
                    {
                        var resources = resourceSets[0].resources;
                        if (resources.Count > 0)
                        {
                            var point = resources[0].point;
                            double latitude = point.coordinates[0];
                            double longitude = point.coordinates[1];

                            Location location = new Location(latitude, longitude);
                            if (!locations.Contains(location))
                            {
                                locations.Add(location);
                                var pin = new Pushpin();

                                MapLayer.SetPosition(pin, location);

                                myMap.Children.Add(pin);
                            }
                            else
                            {
                                RemovePushpin(location);
                                locations.Remove(location);
                                
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error: " + jsonResponse);
                }
            }
        }
        private void RemovePushpin(Location location)
        {
            Pushpin pinToRemove = null;

            // Find the Pushpin in the map's Children collection based on the location
            foreach (var child in myMap.Children)
            {
                if (child is Pushpin pin && MapLayer.GetPosition(pin) == location)
                {
                    pinToRemove = pin;
                    break;
                }
            }

            // Remove the Pushpin from the map
            if (pinToRemove != null)
            {
                myMap.Children.Remove(pinToRemove);
            }
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
              
                HelpProvider.ShowHelp("attractions", mainWindow,1);
                

            }
        }
        private async Task GeocodeAddress(string address)
        {
            // Replace "YourBingMapsAPIKey" with your actual Bing Maps API key
            string apiKey = "yQf68os4c4HuyVo5IUNR~WX58e8NVpKnTDswqWo0zfQ~AojoZD63rjtEfrLRFwN8lH4sTiqHYWvEAdmIF605L3kS8YTmdUZHHTxnqs72XAf2";
            string url = $"http://dev.virtualearth.net/REST/v1/Locations?query={Uri.EscapeDataString(address)}&key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(jsonResponse);
                    var resourceSets = data.resourceSets;
                    if (resourceSets.Count > 0)
                    {
                        var resources = resourceSets[0].resources;
                        if (resources.Count > 0)
                        {
                            var point = resources[0].point;
                            double latitude = point.coordinates[0];
                            double longitude = point.coordinates[1];

                            // Set the center of the map
                            myMap.Center = new Location(latitude, longitude);
                            myMap.ZoomLevel = 10;

                            
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error: " + jsonResponse);
                }
            }
        }


    }
}
