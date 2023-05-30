﻿using HCI.Models.Trips.Model;
using HCI.Models.Trips.Service;
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
        private readonly ITripService _userService;
        public HomePage(ITripService userService)
        {

            _userService = userService;
            InitializeComponent();
            IEnumerable<Trip> trips = _userService.GetAllTrips();
            trips.ToList().ForEach(trip => { tripsStackPanel.Children.Add(new TripCardControl(trip)); });
            


        }

        
    }
}