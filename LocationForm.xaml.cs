using System;
using System.Windows;
using System.Windows.Controls;
using HCI.Models.Locations.Service;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace HCI
{
    public partial class LocationForm : UserControl
    {
        private HCI.Models.Locations.Model.Location existingLocation;
        private ILocationService locationService;
        private Pushpin selectedLocationPin;
        private readonly Action navigateBackToLocations;


        public LocationForm(HCI.Models.Locations.Model.Location location, ILocationService service, Action navigateBackToLocations)
        {
            InitializeComponent();

            existingLocation = location;
            locationService = service;

            this.navigateBackToLocations = navigateBackToLocations;
           
            if (existingLocation != null)
            {
                addressBox.Text = existingLocation.Address;
                cityBox.Text = existingLocation.City;

                saveButton.Content = "Update";

                formLabel.Text = "UPDATE LOCATION";

                MarkLocationOnMap(location);
            }
            map.Loaded += Map_Loaded;
            cancelButton.Click += (sender, e) => navigateBackToLocations?.Invoke();

        }

        public LocationForm(ILocationService service, Action navigateBackToLocations)
        {
            InitializeComponent();

            this.navigateBackToLocations = navigateBackToLocations;

            locationService = service;

            map.Loaded += Map_Loaded;
            cancelButton.Click += (sender, e) => navigateBackToLocations?.Invoke();


        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string address = addressBox.Text;
            string city = cityBox.Text;

            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(city))
            {
                errorMessage.Text = "Please fill in all the fields.";
                errorMessage.Visibility = Visibility.Visible;
                return;
            }

            // Check if the location is within Serbia
            if (!IsLocationInSerbia(address, city))
            {
                errorMessage.Text = "Location must be within Serbia.";
                errorMessage.Visibility = Visibility.Visible;
                return;
            }

            if (existingLocation != null)
            {
                existingLocation.Address = address;
                existingLocation.City = city;

                locationService.UpdateLocation(existingLocation);
                MessageBox.Show("Location updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                navigateBackToLocations?.Invoke();
            }
            else
            {
                HCI.Models.Locations.Model.Location newLocation = new HCI.Models.Locations.Model.Location
                {
                    Address = address,
                    City = city
                };

                locationService.AddLocation(newLocation);
                navigateBackToLocations?.Invoke();
            }

            addressBox.Text = string.Empty;
            cityBox.Text = string.Empty;

            ClearSelectedLocationPin();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            addressBox.Text = string.Empty;
            cityBox.Text = string.Empty;

            ClearSelectedLocationPin();
            navigateBackToLocations?.Invoke();

        }
        private void Map_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(map);
            Location clickedLocation = map.ViewportPointToLocation(mousePosition);

            HCI.Models.Locations.Model.Location reverseGeocodedLocation = ReverseGeocodeLocation(clickedLocation.Latitude, clickedLocation.Longitude);
                
            if (reverseGeocodedLocation != null && IsLocationInSerbia(reverseGeocodedLocation.Address, reverseGeocodedLocation.City))
            {
                SetLocationFormValues(reverseGeocodedLocation);

                HCI.Models.Locations.Model.Location geocodedLocation = new HCI.Models.Locations.Model.Location
                {
                    Address = reverseGeocodedLocation.Address,
                    City = reverseGeocodedLocation.City
                };

                MarkLocationOnMap(geocodedLocation);
                errorMessage.Visibility = Visibility.Collapsed;
            }
            else
            {
                errorMessage.Text = "Selected location must be within Serbia.";
                errorMessage.Visibility = Visibility.Visible;
            }
        }

        private void ShowOnMapButton_Click(object sender, RoutedEventArgs e)
        {
            string address = addressBox.Text;
            string city = cityBox.Text;

            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(city))
            {
                errorMessage.Text = "Please fill in all the fields.";
                errorMessage.Visibility = Visibility.Visible;
                return;
            }

            if (!IsLocationInSerbia(address, city))
            {
                errorMessage.Text = "Location must be within Serbia.";
                errorMessage.Visibility = Visibility.Visible;
                return;
            }

            HCI.Models.Locations.Model.Location location = new HCI.Models.Locations.Model.Location
            {
                Address = address,
                City = city
            };

            errorMessage.Visibility= Visibility.Collapsed;
            MarkLocationOnMap(location);
        }

        private void ZoomMapToSerbia()
        {
            LocationRect serbiaBounds = new LocationRect(new Location(41.8661, 18.8299), new Location(46.181, 23.0066));

            map.SetView(serbiaBounds);
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            ZoomMapToSerbia();
        }

        private bool IsLocationInSerbia(string address, string city)
        {
            RestClient client = new RestClient("https://api.opencagedata.com/");
            RestRequest request = new RestRequest("geocode/v1/json");
            request.AddParameter("q", $"{address}, {city}");
            request.AddParameter("key", "eec32520d36c4997bd6d343e47c0c0da"); // Replace with your actual API key

            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {

                var results = JObject.Parse(response.Content)["results"];
                if (results != null && results.HasValues)
                {
                    var firstResult = results.First;

                    var components = firstResult["components"];
                    if (components != null && components["country"].ToString().Equals("Serbia", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private HCI.Models.Locations.Model.Location ReverseGeocodeLocation(double latitude, double longitude)
        {
            RestClient client = new RestClient("http://dev.virtualearth.net/");
            RestRequest request = new RestRequest("REST/v1/Locations/" + latitude + "," + longitude);
            request.AddParameter("key", "yQf68os4c4HuyVo5IUNR~WX58e8NVpKnTDswqWo0zfQ~AojoZD63rjtEfrLRFwN8lH4sTiqHYWvEAdmIF605L3kS8YTmdUZHHTxnqs72XAf2"); // Replace with your actual API key

            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                dynamic data = JObject.Parse(response.Content);
                string address = data.resourceSets[0].resources[0].address.formattedAddress;
                string city = data.resourceSets[0].resources[0].address.locality;

                return new HCI.Models.Locations.Model.Location { Address = address, City = city };
            }

            return null;
        }

        private void MarkLocationOnMap(HCI.Models.Locations.Model.Location location)
        {
            ClearSelectedLocationPin();

            Pushpin pin = new Pushpin();
            Location onMapLocation = GetLocationFromAddress(location.Address+","+location.City);
            if (onMapLocation == null)
            {
                errorMessage.Text = "Invalid address!";
                errorMessage.Visibility = Visibility.Visible;
                return;
            }
            pin.Location = onMapLocation;
            pin.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);

            map.Children.Add(pin);
            selectedLocationPin = pin;

            map.SetView(pin.Location, 15);
        }

        private void ClearSelectedLocationPin()
        {
            if (selectedLocationPin != null)
            {
                map.Children.Remove(selectedLocationPin);
                selectedLocationPin = null;
            }
        }

        private void SetLocationFormValues(HCI.Models.Locations.Model.Location location)
        {
            addressBox.Text = location.Address;
            cityBox.Text = location.City;
        }

        private Location GetLocationFromAddress(string address)
        {
            RestClient client = new RestClient("https://api.opencagedata.com/");
            RestRequest request = new RestRequest("geocode/v1/json");
            request.AddParameter("q", address);
            request.AddParameter("key", "eec32520d36c4997bd6d343e47c0c0da"); // Replace with your actual API key

            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                dynamic data = JObject.Parse(response.Content);
                double latitude = data.results[0].geometry.lat;
                double longitude = data.results[0].geometry.lng;

                return new Location(latitude, longitude);
            }

            return null;
        }
    }
}
