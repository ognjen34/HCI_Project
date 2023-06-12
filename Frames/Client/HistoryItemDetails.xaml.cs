using HCI.Models.Pictures.Model;
using HCI.Models.Trips.Model;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HCI.Models.Restaurants.Model;
using HCI.Models.Attractions.Model;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Threading;
using HCI.Models.Users.Model;
using HCI.Models.Trips.Service;

namespace HCI.Frames.Client
{
    /// <summary>
    /// Interaction logic for HistoryItemDetails.xaml
    /// </summary>
    public partial class HistoryItemDetails : Page
    {
        private OrderedTrip _trip;
        public int SelectedImg { get; set; }
        private readonly IOrderedTripService _orderedTripService;
        public List<Picture> Pictures { get; set; }
        public User _user;
        public HistoryItemDetails(OrderedTrip trip, User user, IOrderedTripService orderedTripService)
        {
            _user = user;
            _trip = trip;
            _orderedTripService = orderedTripService;
            InitializeComponent();
            GeocodeAddress(_trip.Trip.Accommodation.Location.Address);
            AddPins();
            SelectedImg = 1;
            Pictures = new List<Picture>();
            Pictures = _trip.Trip.Accommodation.Pictures.ToList();
            Pictures.Add(_trip.Trip.Picture);
            SetImageSource(Pictures[SelectedImg].Pictures);
            Console.WriteLine(_trip.Attractions.Count + _trip.Restaurants.Count);
            AddAttraction();
            AddText();
            



        }

        private void AddText()
        {
            AccomodationName.Text = _trip.Trip.Accommodation.Name;
            AccomodationDescritpion.Text = _trip.Trip.Accommodation.Description;
            TripName.Text = _trip.Trip.Name;
            TotalPrice.Text = _trip.TotalPrice.ToString();
            CheckIn.Text = _trip.CheckIn.ToString();
            CheckOut.Text = _trip.CheckOut.ToString();
            Location.Text = _trip.Trip.Accommodation.Location.Address;
            
        }

        private void AddAttraction()
        {
            foreach (var item in _trip.Attractions)
            {
                var attractionItem = new AttractionItem(item,null, _user, _orderedTripService, _trip.Trip);
                attractionItem.ItemClicked += AttractionItem_Clicked;
                attractionItemsControl.Items.Add(attractionItem);
            }
            foreach (var item in _trip.Restaurants)
            {
                var attractionItem = new AttractionItem(null, item, _user, _orderedTripService, _trip.Trip);
                attractionItem.ItemClicked += AttractionItem_Clicked;
                attractionItemsControl.Items.Add(attractionItem);
            }
        }
        private void AttractionItem_Clicked(object sender, EventArgs e)
        {
            var attractionItem = (AttractionItem)sender;
            
            if (attractionItem.AttractionData != null)
            {
                
                GeocodeAddress(attractionItem.AttractionData.Location.Address);

            }
            else
            {


                GeocodeAddress(attractionItem.RestaurantData.Location.Address);

            }
            attractionItem.RemoveGlowEffect();
        }
        private void Location_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
               
                GeocodeAddress(Location.Text);
            
        }

        private void AddPins()
        {
            foreach (Restaurant r in _trip.Restaurants)
            {
                AddPin(r.Location.Address);
            }
            foreach (Attraction r in _trip.Attractions)
            {
                AddPin(r.Location.Address);
            }
        }
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedImg == 0)
            {
                SelectedImg = Pictures.Count - 1;
                SetImageSource(Pictures[SelectedImg].Pictures);
            }
            else
            {
                SelectedImg = SelectedImg - 1; SetImageSource(Pictures[SelectedImg].Pictures);
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedImg == Pictures.Count - 1)
            {
                SelectedImg = 0;
                SetImageSource((Pictures[SelectedImg].Pictures));
            }
            else
            {
                SelectedImg = SelectedImg + 1; SetImageSource((Pictures[SelectedImg].Pictures));
            }

        }
        private void SetImageSource(string base64Image)
        {
            try
            {
                // Create a BitmapImage from the base64 string
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new System.IO.MemoryStream(Convert.FromBase64String(base64Image));
                bitmapImage.EndInit();

                // Set the BitmapImage as the source of the Image control
                accomendationImages.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error setting image source: " + ex.Message);
            }
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

                         
                                var pin = new Pushpin();

                                MapLayer.SetPosition(pin, new Location(latitude,longitude));

                                myMap.Children.Add(pin);
                            
                            
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error: " + jsonResponse);
                }
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
                            myMap.Center = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
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
