
using HCI.Frames.Client;
using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Repository;
using HCI.Models.Accommodations.Service;
using HCI.Models.Attractions.Service;
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

        public MainWindow(IUserService userService, ITripService tripService,IAttractionService attractionService,IRestaurantService restaurantService)
        {
            _userService = userService;
            _restaurantService = restaurantService;
            _tripService = tripService;
            _attractionService = attractionService;
            InitializeComponent();

            var loginForm = new LoginForm(userService);


            contentControl.Navigate(loginForm);
            loginForm.LoginSuccess += LoginForm_LoginSuccess;

        }

        private void LoginForm_LoginSuccess(object sender, LoginSuccessArgs e)
        {
            User user = e.User;
            int userId = user.Id;
            string userType = user.Type.ToString();
            contentControl.Content = null;

            if (user.Type == UserType.Client)
            {
                navbarControl.Content = new ClientNavBar();
                navbarViewBox.Visibility = Visibility.Visible;
                contentControl.Navigate(new Attractions(_attractionService, _restaurantService));

            }
            else if (user.Type == UserType.Agent)
            {
                navbarControl.Content = new AgentNavBar();
                navbarViewBox.Visibility = Visibility.Visible;

            }
            MessageBox.Show($"Login successful! User ID: {userId}, User Type: {userType}");
        }

    }
}