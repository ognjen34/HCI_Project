using HCI.Models.Restaurants.DTO;
using HCI.Models.Restaurants.Model;
using HCI.Models.Restaurants.Service;
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
    /// Interaction logic for RestaurantCardControl.xaml
    /// </summary>
    public partial class RestaurantCardControl : UserControl
    {
        public Restaurant Restaurant { get; }
        public event EventHandler<EditRestaurantArgs> EditClickedEvent;
        private readonly IRestaurantService _restaurantService;

        public RestaurantCardControl(Restaurant restaurant, IRestaurantService restaurantService)
        {
            InitializeComponent();
            _restaurantService = restaurantService;
            Restaurant = restaurant;
            DataContext = Restaurant;
            editButton.Click += EditClicked;

            // Load the base64 image
            LoadBase64Image();
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            EditClickedEvent?.Invoke(this, new EditRestaurantArgs(this.Restaurant));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this restaurant?,this action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _restaurantService.Delete(this.Restaurant.Id);

                var parentStackPanel = Parent as StackPanel;
                parentStackPanel?.Children.Remove(this);
            }
        }

        private void LoadBase64Image()
        {
            if (Restaurant.Picture != null)
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(Restaurant.Picture.Pictures);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new System.IO.MemoryStream(imageBytes);
                    bitmap.EndInit();
                    restaurantImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    // Handle the exception if unable to load the image
                    Console.WriteLine("Error loading image: " + ex.Message);
                }
            }
        }
    }
}
