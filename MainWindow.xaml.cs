
using HCI.Frames;
using HCI.Frames.Client;
using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Repository;
using HCI.Models.Accommodations.Service;
using HCI.Models.Attractions.Service;
using HCI.Models.Locations.Service;
using HCI.Models.Pictures.Service;
using HCI.Models.Restaurants.Service;
using HCI.Models.Trips.Model;
using HCI.Models.Trips.Service;
using HCI.Models.Users.DTO;

using HCI.Models.Users.Model;
using HCI.Models.Users.Service;
using HCI.Navbars;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HCI
{
    public partial class MainWindow : Window
    {
        private readonly IUserService _userService;
        private readonly ITripService _tripService;
        private readonly IAttractionService _attractionService;
        private readonly IRestaurantService _restaurantService;
        private readonly IAccommodationService _accommodationService;
        private readonly IPictureService _pictureService;
        private readonly ILocationService _locationService;



        private User user;

        public MainWindow(IUserService userService, IPictureService pictureService,IAccommodationService accommodationService,ITripService tripService,IAttractionService attractionService,IRestaurantService restaurantService, ILocationService locationService)
        {
            user = null;
            _pictureService = pictureService;
            _userService = userService;
            _restaurantService = restaurantService;
            _tripService = tripService;
            _attractionService = attractionService;
            _locationService = locationService;
            _accommodationService = accommodationService;
            InitializeComponent();

            var loginForm = new LoginForm(userService);


            contentControl.Navigate(loginForm);
            loginForm.LoginSuccess += LoginForm_LoginSuccess;
            loginForm.RegisterPressed += RegisterClicked;

        }

        private void LoginForm_LoginSuccess(object sender, LoginSuccessArgs e)
        {
            user = e.User;

            contentControl.Content = null;

            if (user.Type == UserType.Client)
            {
                ClientNavBar clientNavBar = new ClientNavBar();
                navbarControl.Content = clientNavBar;
                clientNavBar.HomeClicked += HomeClicked;
                clientNavBar.LogoutClicked += LogoutClicked;
                navbarViewBox.Visibility = Visibility.Visible;

                contentControl.Navigate(new HomePage(_tripService,_pictureService, _accommodationService,_attractionService, _restaurantService,user));

            }
            else if (user.Type == UserType.Agent)
            {
                AgentNavBar agentNavBar = new AgentNavBar();
                agentNavBar.HomeClicked += HomeClicked;
                agentNavBar.LocationClicked += LocationClicked;
                agentNavBar.LogoutClicked += LogoutClicked;
                agentNavBar.RestaurantClicked += RestaurantClicked;

                navbarControl.Content = agentNavBar;
                navbarViewBox.Visibility = Visibility.Visible;
                contentControl.Navigate(new HomePage(_tripService, _pictureService, _accommodationService, _attractionService, _restaurantService, user));

            }
        }

        public void NavigateToHome()
        {
            contentControl.Navigate(new HomePage(_tripService, _pictureService, _accommodationService, _attractionService, _restaurantService, user));

        }
        private void HomeClicked(object sender, EventArgs e)
        {
            NavigateToHome();   
        }

        public void NavigateToLocationsAgent()
        {
           contentControl.Navigate(new LocationPage(_locationService));
        }
        private void LocationClicked(object sender, EventArgs e)
        {
            NavigateToLocationsAgent();
        }


        private void RegisterClicked(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm(_userService);
            registerForm.buttonClicked += navigateBackToLogin;
            contentControl.Navigate(registerForm);
        }

        private void navigateBackToLogin(object sender, EventArgs e)
        {
            var loginForm = new LoginForm(_userService);
            contentControl.Navigate(loginForm);
            loginForm.LoginSuccess += LoginForm_LoginSuccess;
            loginForm.RegisterPressed += RegisterClicked;

        public void NavigateToRestaurantsAgent()
        {
            contentControl.Navigate(new RestaurantPage(_restaurantService,_locationService,_pictureService));
        }
        private void RestaurantClicked(object sender, EventArgs e)
        {
            NavigateToRestaurantsAgent();

        }

        private void LogoutClicked(object sender, EventArgs e)
        {
            user = null;
            navbarControl.Content = null;
            navbarViewBox.Visibility = Visibility.Collapsed;

            var loginForm = new LoginForm(_userService);
            contentControl.Navigate(loginForm);
            loginForm.LoginSuccess += LoginForm_LoginSuccess;
            loginForm.RegisterPressed += RegisterClicked;
        }

    }


}