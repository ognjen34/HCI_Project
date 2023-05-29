
﻿using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Repository;
using HCI.Models.Accommodations.Service;
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

        public MainWindow(IUserService userService, ITripService tripService)
        {
            _userService = userService;
            _tripService = tripService;
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
                contentControl.Navigate(new HomePage(_tripService));

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