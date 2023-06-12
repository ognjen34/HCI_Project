using HCI.Models.Locations.Service;
using HCI.Models.Pictures.Service;
using HCI.Models.Restaurants.DTO;
using HCI.Models.Restaurants.Model;
using HCI.Models.Restaurants.Service;
using HCI.Tools;
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
    /// Interaction logic for RestaurantPage.xaml
    /// </summary>
    public partial class RestaurantPage : Page
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ILocationService _locationService;
        private readonly IPictureService _pictureService;


        public RestaurantPage(IRestaurantService restaurantService,ILocationService locationService,IPictureService pictureService)
        {
            _locationService = locationService;
            _pictureService = pictureService;
            _restaurantService = restaurantService;
            InitializeComponent();
            IEnumerable<Restaurant> restaurants = _restaurantService.GetAll();
            foreach (Restaurant restaurant in restaurants)
            {
                RestaurantCardControl restaurantCard = new RestaurantCardControl(restaurant, _restaurantService);
                restaurantsStackPanel.Children.Add(restaurantCard);
                restaurantCard.EditClickedEvent += RestaurantCardControl_EditClicked;
            }
        }

        private void RestaurantCardControl_EditClicked(object sender, EditRestaurantArgs e)
        {
            Restaurant restaurant = e.restaurant;
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new RestaurantForm(restaurant, _restaurantService,_locationService,_pictureService, () => mainWindow.NavigateToRestaurantsAgent()));
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                HelpProvider.ShowHelp("agentrestaurants", mainWindow, 2);
                
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new RestaurantForm(null, _restaurantService, _locationService, _pictureService, () => mainWindow.NavigateToRestaurantsAgent()));
        }
    }
}
