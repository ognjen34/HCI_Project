using HCI.Models.Users.Model;
using HCI.Models.Users.Service;
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
            List<User> users = (List<User>)userService.GetAllUsers();
            Console.WriteLine(users[0].Type);
            Console.WriteLine(users[1].Type);


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Handle button click events

            // Example: Writing to the console
            Console.WriteLine("Button clicked!");
        }
    }
}