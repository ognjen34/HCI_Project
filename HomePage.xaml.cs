﻿using HCI.Models.Attractions.Service;
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
using HCI.Tools;

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
        private readonly IOrderedTripService _orderedTripService;

        private readonly User _user;

        public HomePage(ITripService tripService, IPictureService pictureService, IAccommodationService accomodationService, IAttractionService attractionService, IRestaurantService restaurantService,IOrderedTripService orderedTripService, User user)
        {
            _pictureService = pictureService;
            _tripService = tripService;
            _attractionService = attractionService;
            _restaurantService = restaurantService;
            _accommodationService = accomodationService;
            _orderedTripService = orderedTripService;

            _user = user;

            InitializeComponent();
            IEnumerable<Trip> trips = _tripService.GetAllTrips();

            if (_user.Type == UserType.Agent)
            {
                trips.ToList().ForEach(trip => { tripsStackPanel.Children.Add(new TripCardControl(trip,UserType.Agent,_tripService)); });
                foreach (TripCardControl tripCardControl in tripsStackPanel.Children)
                {
                    tripCardControl.EditClickedEvent += TripCardControl_EditClicked;
                }
                addButton.Visibility = Visibility.Visible;
            }
            else
            {
                trips.ToList().ForEach(trip => { tripsStackPanel.Children.Add(new TripCardControl(trip,UserType.Client,_tripService)); });
                foreach (TripCardControl tripCardControl in tripsStackPanel.Children)
                {
                    tripCardControl.OrderTrip += TripCardControl_OrderClicked;
                }
                addButton.Visibility = Visibility.Collapsed;
            }

            
        }

        private void TripCardControl_OrderClicked(object sender, OrderTripArgs e)
        {
            Trip trip = e.trip;
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new CertainTripPage(trip, _attractionService, _restaurantService, _orderedTripService,_user));
        }

        private void TripCardControl_EditClicked(object sender, OrderTripArgs e)
        {
            Trip trip = e.trip;
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new TripForm(trip,_tripService, _accommodationService, _pictureService, () => mainWindow.NavigateToHome()));
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if(_user.Type == UserType.Agent)
                {
                    HelpProvider.ShowHelp("agenthome", mainWindow);
                }
                else
                {
                    HelpProvider.ShowHelp("clienthome", mainWindow);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new TripForm(null,_tripService, _accommodationService, _pictureService, () => mainWindow.NavigateToHome()));
        }
    }
}
