using HCI.Models.Locations.Model;
using HCI.Models.Locations.Service;
using HCI.Models.Pictures.Model;
using HCI.Models.Pictures.Service;
using HCI.Models.Restaurants.Model;
using HCI.Models.Restaurants.Service;
using HCI.Tools;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace HCI
{
    /// <summary>
    /// Interaction logic for RestaurantForm.xaml
    /// </summary>
    public partial class RestaurantForm : UserControl
    {
        private readonly Restaurant existingRestaurant;
        private readonly IRestaurantService restaurantService;
        private readonly ILocationService locationService;
        private readonly IPictureService pictureService;
        private readonly Action navigateBackToRestaurants;
        private byte[] selectedImageBytes;

        public RestaurantForm(Restaurant restaurant, IRestaurantService restaurantService, ILocationService locationService, IPictureService pictureService, Action navigateBackToRestaurants)
        {
            InitializeComponent();

            existingRestaurant = restaurant;
            this.restaurantService = restaurantService;
            this.locationService = locationService;
            this.pictureService = pictureService;
            this.navigateBackToRestaurants = navigateBackToRestaurants;


            cuisineTypeComboBox.ItemsSource = Enum.GetValues(typeof(CuisineType));
            cuisineTypeComboBox.SelectedItem = CuisineType.Italian;

            if (existingRestaurant != null)
            {
                nameBox.Text = existingRestaurant.Name;
                descriptionBox.Text = existingRestaurant.Description;
                ratingBox.Text = existingRestaurant.Rating.ToString();
                cuisineTypeComboBox.SelectedItem = existingRestaurant.CuisineType;

                if (existingRestaurant.Picture != null)
                {
                    selectedImageBytes = Convert.FromBase64String(existingRestaurant.Picture.Pictures);
                    selectedImage.Source = LoadImage(selectedImageBytes);
                    imagePlaceholder.Visibility = Visibility.Collapsed;
                }

                formLabel.Text = "UPDATE RESTAURANT";
            }

            cancelButton.Click += (sender, e) => navigateBackToRestaurants?.Invoke();
            nextButton.Click += NextButton_Click;

            imagePreview.Drop += ImagePreview_Drop;
            imagePreview.DragEnter += ImagePreview_DragEnter;
            imagePreview.DragLeave += ImagePreview_DragLeave;
            imagePreview.DragOver += ImagePreview_DragOver;
        }



        private BitmapImage LoadImage(byte[] imageData)
        {
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text.Trim();
            string description = descriptionBox.Text.Trim();
            string ratingText = ratingBox.Text.Trim();
            CuisineType selectedCuisineType = (CuisineType)cuisineTypeComboBox.SelectedItem;

            if (string.IsNullOrWhiteSpace(name))
            {
                errorMessageTextBlock.Text = "Please fill in the Name field.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                errorMessageTextBlock.Text = "Please fill in the Description field.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrWhiteSpace(ratingText))
            {
                errorMessageTextBlock.Text = "Please fill in the Rating field with a value between 1 and 5.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (selectedImageBytes == null)
            {
                errorMessageTextBlock.Text = "Please drag and drop an image to the Picture box.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }


            if (name.Length > 30)
            {
                errorMessageTextBlock.Text = "Name must be no more than 30 characters.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (!double.TryParse(ratingText, out double rating) || rating < 1 || rating > 5)
            {
                errorMessageTextBlock.Text = "Please enter a valid rating between 1 and 5.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            Restaurant restaurant = existingRestaurant ?? new Restaurant();
            restaurant.Name = name;
            restaurant.Description = description;
            restaurant.Rating = rating;
            restaurant.CuisineType = selectedCuisineType;
            restaurant.ClassName = "Restaurant";

            if (selectedImageBytes != null)
            {
                if (existingRestaurant != null)
                {
                    Picture picture = new Picture { Pictures = Convert.ToBase64String(selectedImageBytes) };
                    restaurant.Picture = picture;
                    pictureService.DeletePicture(restaurant.Picture.Id);
                }
                else
                {
                    Picture picture = new Picture { Pictures = Convert.ToBase64String(selectedImageBytes) };
                    restaurant.Picture = picture;
                }
            }

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new LocationForm(restaurant, locationService, restaurantService, () => mainWindow.contentControl.Navigate(new RestaurantForm(restaurant, restaurantService, locationService,pictureService, () => mainWindow.NavigateToRestaurantsAgent())), () => mainWindow.NavigateToRestaurantsAgent()));
        }



        private void ImagePreview_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    selectedImageBytes = File.ReadAllBytes(files[0]);
                    selectedImage.Source = LoadImage(selectedImageBytes);
                    imagePlaceholder.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ImagePreview_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            e.Handled = true;
        }

        private void ImagePreview_DragLeave(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                HelpProvider.ShowHelp("agentrestaurantsform", mainWindow,2);


            }
        }

        private void ImagePreview_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }
    }
}
