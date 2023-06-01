using HCI.Models.Locations.Model;
using HCI.Models.Locations.Service;
using HCI.Models.Pictures.Model;
using HCI.Models.Pictures.Service;
using HCI.Models.Restaurants.Model;
using HCI.Models.Restaurants.Service;
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

            // Set default values for location and cuisine type
            locationComboBox.ItemsSource = locationService.GetAllLocations();
            locationComboBox.SelectedItem = locationComboBox.Items[0];

            cuisineTypeComboBox.ItemsSource = Enum.GetValues(typeof(CuisineType));
            cuisineTypeComboBox.SelectedItem = CuisineType.Italian;

            // Initialize form with existing restaurant data
            if (existingRestaurant != null)
            {
                nameBox.Text = existingRestaurant.Name;
                descriptionBox.Text = existingRestaurant.Description;
                ratingBox.Text = existingRestaurant.Rating.ToString();
                locationComboBox.SelectedItem = existingRestaurant.Location;
                cuisineTypeComboBox.SelectedItem = existingRestaurant.CuisineType;

                if (existingRestaurant.Picture != null)
                {
                    selectedImageBytes = Convert.FromBase64String(existingRestaurant.Picture.Pictures);
                    selectedImage.Source = LoadImage(selectedImageBytes);
                    imagePlaceholder.Visibility = Visibility.Collapsed;
                }

                saveButton.Content = "Update";
                formLabel.Text = "UPDATE RESTAURANT";
            }

            cancelButton.Click += (sender, e) => navigateBackToRestaurants?.Invoke();
            saveButton.Click += SaveButton_Click;

            // Allow drag and drop for the image
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text.Trim();
            string description = descriptionBox.Text.Trim();
            string ratingText = ratingBox.Text.Trim();
            Location selectedLocation = (Location)locationComboBox.SelectedItem;
            CuisineType selectedCuisineType = (CuisineType)cuisineTypeComboBox.SelectedItem;

            // Validate form inputs
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(ratingText) || selectedLocation == null || selectedImageBytes == null)
            {
                errorMessageTextBlock.Text = "Please fill in all the fields.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            // Validate name length
            if (name.Length > 30)
            {
                errorMessageTextBlock.Text = "Name must be no more than 30 characters.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            // Validate rating
            if (!double.TryParse(ratingText, out double rating) || rating < 0 || rating > 5)
            {
                errorMessageTextBlock.Text = "Please enter a valid rating between 0 and 5.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            // Create new or update existing restaurant
            Restaurant restaurant = existingRestaurant ?? new Restaurant();
            restaurant.Name = name;
            restaurant.Description = description;
            restaurant.Rating = rating;
            restaurant.Location = selectedLocation;
            restaurant.CuisineType = selectedCuisineType;
            restaurant.ClassName = "Restaurant";

            if (selectedImageBytes != null)
            {
                if (existingRestaurant != null)
                {

                    Picture picture = new Picture { Pictures = Convert.ToBase64String(selectedImageBytes) };
                    restaurant.Picture = picture;
                    pictureService.DeletePicture(existingRestaurant.Picture.Id);
                }

            }

            if (existingRestaurant == null)
            {
                restaurantService.Add(restaurant);
            }
            else
            {
                restaurantService.Update(restaurant);
            }

            navigateBackToRestaurants?.Invoke();
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
