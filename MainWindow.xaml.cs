using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Repository;
using HCI.Models.Accommodations.Service;
using HCI.Models.Trips.Model;
using HCI.Models.Trips.Service;
using HCI.Models.Users.Model;
using HCI.Models.Users.Service;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HCI
{
    public partial class MainWindow : Window
    {
        private readonly ITripService _userService;

        public MainWindow(ITripService userService)
        {
            _userService = userService;
            InitializeComponent();
            Trip trip = _userService.GetById(1);
            Console.WriteLine(trip.Name);
            Console.WriteLine(trip.Description);
            SetImageSource(trip.Picture.Pictures);



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Handle button click events

            // Example: Writing to the console
            Console.WriteLine("Button clicked!");
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

    }
}