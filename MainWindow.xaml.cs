using HCI.Models.Users.DTO;
using HCI.Models.Users.Model;
using HCI.Models.Users.Service;
using HCI.Navbars;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HCI
{
    public partial class MainWindow : Window
    {
        private readonly IUserService _userService;

        public MainWindow(IUserService userService)
        {
            _userService = userService;
            InitializeComponent();

            var loginForm = new LoginForm(userService);

            contentControl.Content = loginForm;
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