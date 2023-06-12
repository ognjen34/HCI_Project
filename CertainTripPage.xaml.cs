using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using HCI.Models.Accommodations.Model;
using Microsoft.Maps.MapControl.WPF;
using HCI.Models.Pictures.Model;
using HCI.Models.Trips.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HCI.Models.Attractions.Service;
using HCI.Models.Restaurants.Service;
using HCI.Frames.Client;
using HCI.Models.Trips.Service;
using HCI.Models.Users.Model;
using HCI.Tools;

namespace HCI
{
    /// <summary>
    /// Interaction logic for CertainTripPage.xaml
    /// </summary>
    public partial class CertainTripPage : Page
    {
        public Trip Trip { get; set; }
        public int SelectedImg { get; set; }
        public List<Picture> Pictures { get; set; }
        private readonly IAttractionService _attractionService;
        private readonly IRestaurantService _restaurantService;
        private readonly IOrderedTripService _orderedTripService;
        private User _user;
        public CertainTripPage(Trip trip, IAttractionService attractionService, IRestaurantService restaurantService,IOrderedTripService orderedTripService,User user)
        {
            
            _attractionService = attractionService;
            _restaurantService = restaurantService;
            _orderedTripService = orderedTripService;
            _user = user;
            Trip = trip;
            SelectedImg = 1;
            Pictures = new List<Picture>();
            Pictures = Trip.Accommodation.Pictures.ToList();
            InitializeComponent();

            errorMessage.Visibility = Visibility.Collapsed;

            GeocodeAddress(Trip.Accommodation.Location.Address);
            tripImage.Source = StrToBit(trip.Picture.Pictures);
            accomendationLocation.Text = Trip.Accommodation.Location.City + " " + Trip.Accommodation.Location.Address; 
            accomendationName.Text = trip.Accommodation.Name;
            tripName.Text = trip.Name;
            tripDescription.Text = trip.Description;
            accomendationPrice.Text = "Price: " + trip.Accommodation.PricePerDay.ToString() + "$/night";
            accomendationBeds.Text = "Beds:" + trip.Accommodation.Beds.ToString();


            SetImageSource(Pictures[SelectedImg].Pictures);
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
        private BitmapImage StrToBit(string str)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new System.IO.MemoryStream(Convert.FromBase64String(str));
            bitmapImage.EndInit();
            return bitmapImage;

        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                HelpProvider.ShowHelp("tripdetails", mainWindow, 1);


            }
        }

        private void SetImageSource(string base64Image)
        {
            try
            {
                

                // Set the BitmapImage as the source of the Image control
                accomendationImages.Source = StrToBit(base64Image);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error setting image source: " + ex.Message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            errorMessage.Text = message;
            errorMessage.Visibility = Visibility.Visible;
        }

        private void resrveButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkInDate.SelectedDate == null || checkoutDate.SelectedDate == null)
            {
                ShowErrorMessage("Please select both check-in and check-out dates.");
                return;
            }

            DateTime checkIn = checkInDate.SelectedDate.Value.Date;
            DateTime checkOut = checkoutDate.SelectedDate.Value.Date;

            if (checkIn < DateTime.Today)
            {
                ShowErrorMessage("Check-in date cannot be in the past.");
                return;
            }

            if (checkOut <= checkIn)
            {
                ShowErrorMessage("Check-out date must be after the check-in date.");
                return;
            }

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            OrderedTrip orderedTrip = new OrderedTrip();
            orderedTrip.Trip = Trip;
            orderedTrip.User = _user;
            orderedTrip.CheckOut = checkoutDate.SelectedDate ?? DateTime.MinValue;
            orderedTrip.CheckIn = checkInDate.SelectedDate ?? DateTime.MinValue;
            
            mainWindow.contentControl.Navigate(new Attractions(_attractionService, _restaurantService, _orderedTripService, orderedTrip,_user));
        }


        private void checkInDate_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            datePicker.DisplayDateStart = DateTime.Today.AddDays(1);
        }
        private void checkOutDate_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            datePicker.DisplayDateStart = DateTime.Today.AddDays(2);
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
                            myMap.ZoomLevel = 15;

                            // Add a pin to the map
                            var pin = new Pushpin();
                            MapLayer.SetPosition(pin, new Location(latitude, longitude));
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
    }
}
