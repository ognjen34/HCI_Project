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
using HCI.Models.Locations.Model;
using HCI.Models.Pictures.Model;
using HCI.Models.Pictures.Service;
using HCI.Models.Attractions.Model;
using HCI.Models.Attractions.Service;
using HCI.Models.Locations.Service;

namespace HCI
{
    /// <summary>
    /// Interaction logic for AttractionForm.xaml
    /// </summary>
    public partial class AttractionForm : UserControl
    {
        private readonly Attraction existingAttraction;
        private readonly IPictureService pictureService;
        private readonly IAttractionService attractionService;
        private readonly ILocationService locationService;

        private readonly Action navigateBackToAttractions;
        private byte[] selectedImageBytes;

        public AttractionForm(Attraction attraction, ILocationService locationService, IPictureService pictureService, IAttractionService attractionService, Action navigateBackToAttractions)
        {
            InitializeComponent();

            existingAttraction = attraction;
            this.pictureService = pictureService;
            this.attractionService = attractionService;
            this.locationService = locationService;

            // Initialize form with existing attraction data
            if (existingAttraction != null)
            {
                nameBox.Text = existingAttraction.Name;
                descriptionBox.Text = existingAttraction.Description;
                priceBox.Text = existingAttraction.Price.ToString();

                if (existingAttraction.Picture != null)
                {
                    selectedImageBytes = Convert.FromBase64String(existingAttraction.Picture.Pictures);
                    selectedImage.Source = LoadImage(selectedImageBytes);
                    imagePlaceholder.Visibility = Visibility.Collapsed;
                }

                formLabel.Text = "UPDATE ATTRACTION";
            }

            cancelButton.Click += (sender, e) => navigateBackToAttractions?.Invoke();
            nextButton.Click += NextButton_Click;

            imagePreview.Drop += ImagePreview_Drop;
            imagePreview.DragEnter += ImagePreview_DragEnter;
            imagePreview.DragLeave += ImagePreview_DragLeave;
            imagePreview.DragOver += ImagePreview_DragOver;
            this.navigateBackToAttractions += navigateBackToAttractions;
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
            string priceText = priceBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                errorMessageTextBlock.Text = "Please fill in the Name field.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if ( string.IsNullOrWhiteSpace(description))
            {
                errorMessageTextBlock.Text = "Please fill in the Description field.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrWhiteSpace(priceText))
            {
                errorMessageTextBlock.Text = "Please fill in the Price field with valid price number in euros.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (selectedImageBytes == null)
            {
                errorMessageTextBlock.Text = "Please drag and drop an image to the Picture box.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (!float.TryParse(priceText, out float price) || price < 0)
            {
                errorMessageTextBlock.Text = "Please enter a valid price greater than or equal to 0.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            if (name.Length > 30)
            {
                errorMessageTextBlock.Text = "Name must be no more than 30 characters.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            this.imagePlaceholder.Visibility = Visibility.Collapsed;

            Attraction attraction = existingAttraction ?? new Attraction();
            attraction.Name = name;
            attraction.Description = description;
            attraction.Price = price;

            if (selectedImageBytes != null)
            {
                if (existingAttraction != null)
                {
                    Picture picture = new Picture { Pictures = Convert.ToBase64String(selectedImageBytes) };
                    attraction.Picture = picture;
                    pictureService.DeletePicture(existingAttraction.Picture.Id);
                }
                else
                {
                    Picture picture = new Picture { Pictures = Convert.ToBase64String(selectedImageBytes) };
                    attraction.Picture = picture;
                }
            }
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new LocationForm(attraction, locationService, attractionService, () => mainWindow.contentControl.Navigate(new AttractionForm(attraction, locationService, pictureService, attractionService, () => mainWindow.NavigateToAttractionsAgent())), () => mainWindow.NavigateToAttractionsAgent()));
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
