using HCI.Models.Attractions.Service;
using HCI.Models.Restaurants.Service;
﻿using HCI.Models.Accommodations.Service;
using HCI.Models.Pictures.Service;
using HCI.Models.Trips.DTO;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        private readonly ITripService _tripService;
        private readonly IAccommodationService _accommodationService;
        private readonly IPictureService _pictureService;
        private readonly IAttractionService _attractionService;
        private readonly IRestaurantService _restaurantService;

        private readonly User _user;

        public HomePage(ITripService tripService, IPictureService pictureService, IAccommodationService accomodationService, IAttractionService attractionService, IRestaurantService restaurantService, User user)
        {
            _pictureService = pictureService;
            _tripService = tripService;
            _attractionService = attractionService;
            _restaurantService = restaurantService;
            _accommodationService = accomodationService;

            _user = user;

            InitializeComponent();

            if (_user.Type == UserType.Agent)
            {
                addButton.Visibility = Visibility.Visible;
            }
            else
            {
                addButton.Visibility = Visibility.Collapsed;
            }

            IEnumerable<Trip> trips = _tripService.GetAllTrips();
            trips.ToList().ForEach(trip => { tripsStackPanel.Children.Add(new TripCardControl(trip)); });
            foreach (TripCardControl tripCardControl in tripsStackPanel.Children)
            {
                tripCardControl.OrderTrip += TripCardControl_OrderClicked;
            }
        }

        private void TripCardControl_OrderClicked(object sender, OrderTripArgs e)
        {
            Trip trip = e.trip;
            Console.WriteLine(trip.Name);
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new CertainTripPage(trip, _attractionService, _restaurantService));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new TripForm(_tripService, _accommodationService, _pictureService, () => mainWindow.NavigateToHome()));
        }
    }
}
