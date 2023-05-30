using HCI.Models.Attractions.Service;
using HCI.Models.Restaurants.Service;
using HCI.Models.Trips.DTO;
using HCI.Models.Trips.Model;
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
        private readonly IAttractionService _attractionService;
        private readonly IRestaurantService _restaurantService;
        public HomePage(ITripService userService, IAttractionService attractionService, IRestaurantService restaurantService)
        {
            Console.WriteLine("opet opop");
            _attractionService = attractionService;
            _restaurantService = restaurantService;
            _userService = userService;
            InitializeComponent();
            IEnumerable<Trip> trips = _userService.GetAllTrips();
            trips.ToList().ForEach(trip => { tripsStackPanel.Children.Add(new TripCardControl(trip)); });
            foreach(TripCardControl tripCardControl in tripsStackPanel.Children)
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


    }
}
