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

            PopulateLocations();

            // Initialize form with existing attraction data
            if (existingAttraction != null)
            {
                nameBox.Text = existingAttraction.Name;
                descriptionBox.Text = existingAttraction.Description;
                priceBox.Text = existingAttraction.Price.ToString();

                if (existingAttraction.Location != null)
                {
                    StackPanel stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };


                    TextBlock cityTextBlock = new TextBlock
                    {
                        Text = existingAttraction.Location.City
                    };

                    TextBlock addressTextBlock = new TextBlock
                    {
                        Text = existingAttraction.Location.Address
                    };

                    stackPanel.Children.Add(cityTextBlock);
                    stackPanel.Children.Add(addressTextBlock);

                    ComboBoxItem comboBoxItem = new ComboBoxItem
                    {
                        Content = stackPanel,
                        Tag = existingAttraction.Location
                    };

                    locationComboBox.SelectedItem = comboBoxItem;
                }

                if (existingAttraction.Picture != null)
                {
                    selectedImageBytes = Convert.FromBase64String(existingAttraction.Picture.Pictures);
                    selectedImage.Source = LoadImage(selectedImageBytes);
                    imagePlaceholder.Visibility = Visibility.Collapsed;
                }

                saveButton.Content = "Update";
                formLabel.Text = "UPDATE ATTRACTION";
            }

            cancelButton.Click += (sender, e) => navigateBackToAttractions?.Invoke();
            saveButton.Click += SaveButton_Click;

            imagePreview.Drop += ImagePreview_Drop;
            imagePreview.DragEnter += ImagePreview_DragEnter;
            imagePreview.DragLeave += ImagePreview_DragLeave;
            imagePreview.DragOver += ImagePreview_DragOver;
            this.navigateBackToAttractions += navigateBackToAttractions;
        }

        private void PopulateLocations()
        {
            IEnumerable<Location> locations = locationService.GetAllLocations();

            foreach (Location location in locations)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                TextBlock cityTextBlock = new TextBlock
                {
                    Text = location.City
                };

                TextBlock addressTextBlock = new TextBlock
                {
                    Text = location.Address
                };

                stackPanel.Children.Add(cityTextBlock);
                stackPanel.Children.Add(addressTextBlock);

                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = stackPanel,
                    Tag = location
                };

                locationComboBox.Items.Add(comboBoxItem);

                if (existingAttraction != null && existingAttraction.Location != null && existingAttraction.Location.Equals(location))
                {
                    locationComboBox.SelectedItem = comboBoxItem;
                }
            }
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
            string priceText = priceBox.Text.Trim();
            Location selectedLocation = ((ComboBoxItem)locationComboBox.SelectedItem)?.Tag as Location;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(priceText) || selectedLocation == null || selectedImageBytes == null)
            {
                errorMessageTextBlock.Text = "Please fill in all the fields.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            // Validate price
            if (!float.TryParse(priceText, out float price) || price < 0)
            {
                errorMessageTextBlock.Text = "Please enter a valid price greater than or equal to 0.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            Attraction attraction = existingAttraction ?? new Attraction();
            attraction.Name = name;
            attraction.Description = description;
            attraction.Price = price;
            attraction.Location = selectedLocation;

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

            if (existingAttraction == null)
            {
                attractionService.Add(attraction);
            }
            else
            {
                attractionService.Update(attraction);
            }

            navigateBackToAttractions?.Invoke();
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
