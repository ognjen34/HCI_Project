using HCI.Models.Attractions.Service;
using HCI.Models.Restaurants.Service;
using Microsoft.Maps.MapControl.WPF;
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

namespace HCI.Frames.Client
{
    /// <summary>
    /// Interaction logic for Attractions.xaml
    /// </summary>
    public partial class Attractions : UserControl
    {
        IAttractionService attractionService;
        IRestaurantService restaurantService;
        public Attractions(IAttractionService attractionService , IRestaurantService restaurantService)
        {
            this.attractionService = attractionService;
            this.restaurantService = restaurantService;
            InitializeComponent();
            myMap.Center = new Location(47.6097, -122.3331);
            myMap.ZoomLevel = 10;

            // Get the data from both services
            var attractions = attractionService.GetAll();
            var restaurants = restaurantService.GetAll();

            // Merge the data into a single collection
            var mergedItems = new List<object>();
            mergedItems.AddRange(attractions);
            mergedItems.AddRange(restaurants);

            attractionItemsControl.ItemsSource = mergedItems;
        }
    }
}
