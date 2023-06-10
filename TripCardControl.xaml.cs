﻿using HCI.Models.Trips.DTO;
using HCI.Models.Trips.Model;
using HCI.Models.Trips.Service;
using HCI.Models.Users.Model;
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

namespace HCI
{
    /// <summary>
    /// Interaction logic for TripCardControl.xaml
    /// </summary>
    public partial class TripCardControl : UserControl
    {
        public Trip Trip;
        public event EventHandler<OrderTripArgs> OrderTrip;
        public event EventHandler<OrderTripArgs> EditClickedEvent;

        private readonly UserType userType;
        private readonly ITripService _tripService;

        public TripCardControl(Trip trip, UserType userType, ITripService tripService)
        {
            this.userType = userType;
            _tripService = tripService;
            Trip = trip;
            InitializeComponent();
            tripName.Text = trip.Name;
            description.Text = trip.Description;
            bedsValue.Text = trip.Accommodation.Beds.ToString();
            locationName.Text = trip.Accommodation.Location.Address.ToString();
            Console.WriteLine(trip.Name);
            Console.WriteLine(trip.Description);
            SetImageSource(trip.Picture.Pictures);
            accomendationPrice.Text = Trip.Accommodation.PricePerDay.ToString() + "$/Night";
            UpdateButtonVisibility();
        }

        private void UpdateButtonVisibility()
        {
            switch (userType)
            {
                case UserType.Agent:
                    orderButton.Visibility = Visibility.Collapsed;
                    editButton.Visibility = Visibility.Visible;
                    deleteButton.Visibility = Visibility.Visible;
                    break;
                case UserType.Client:
                    orderButton.Visibility = Visibility.Visible;
                    editButton.Visibility = Visibility.Collapsed;
                    deleteButton.Visibility = Visibility.Collapsed;
                    break;
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
                imageControl.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error setting image source: " + ex.Message);
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderTrip?.Invoke(this, new OrderTripArgs(Trip));
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            EditClickedEvent?.Invoke(this, new OrderTripArgs(this.Trip));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this trip? This action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _tripService.RemoveTrip(Trip);

                var parentStackPanel = Parent as StackPanel;
                parentStackPanel?.Children.Remove(this);
            }
        }

        private void EditTrip()
        {
            // Perform the necessary actions for editing the trip
        }
    }
}
