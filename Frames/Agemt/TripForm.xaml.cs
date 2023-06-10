using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Service;
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
using HCI.Models.Pictures.Service;
using HCI.Models.Pictures.Model;
using System.IO;

namespace HCI
{
    /// <summary>
    /// Interaction logic for TripForm.xaml
    /// </summary>
    public partial class TripForm : UserControl
    {
        private readonly ITripService tripService;
        private readonly IAccommodationService accommodationService;
        private readonly IPictureService pictureService;
        private readonly Action navigateBackToHomePage;

        private string base64Image;

        public TripForm(ITripService tripService, IAccommodationService accommodationService, IPictureService pictureService, Action navigateBackToHomePage)
        {
            InitializeComponent();

            this.navigateBackToHomePage = navigateBackToHomePage;
            this.tripService = tripService;
            this.accommodationService = accommodationService;
            this.pictureService = pictureService;

            PopulateAccommodations();

            AllowDrop = true;
            DragEnter += TripForm_DragEnter;
            Drop += TripForm_Drop;
        }

        private async void TripForm_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string filePath = files[0];
                    string extension = System.IO.Path.GetExtension(filePath);

                    if (extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        base64Image = await ConvertImageToBase64(filePath);

                        BitmapImage imageSource = new BitmapImage(new Uri(filePath));
                        imagePreview.Source = imageSource;
                        imagePlaceholder.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void TripForm_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private async Task<string> ConvertImageToBase64(string filePath)
        {
            try
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
                return Convert.ToBase64String(imageBytes);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void PopulateAccommodations()
        {
            IEnumerable<Accommodation> accommodations = accommodationService.GetAllAccommodations();

            foreach (Accommodation accommodation in accommodations)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                TextBlock nameTextBlock = new TextBlock
                {
                    Text = accommodation.Name,
                    FontWeight = FontWeights.Bold
                };

                TextBlock cityTextBlock = new TextBlock
                {
                    Text = accommodation.Location.City
                };

                TextBlock addressTextBlock = new TextBlock
                {
                    Text = accommodation.Location.Address
                };

                stackPanel.Children.Add(nameTextBlock);
                stackPanel.Children.Add(cityTextBlock);
                stackPanel.Children.Add(addressTextBlock);

                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = stackPanel,
                    Tag = accommodation
                };

                accommodationComboBox.Items.Add(comboBoxItem);

                if (accommodationComboBox.SelectedItem == null)
                {
                    accommodationComboBox.SelectedItem = comboBoxItem;
                }
            }
        }

        private void AddTripButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            string description = descriptionBox.Text;
            ComboBoxItem selectedItem = (ComboBoxItem)accommodationComboBox.SelectedItem;
            Accommodation selectedAccomodation = (Accommodation)selectedItem.Tag;

            if (ValidateForm(name, description, selectedAccomodation))
            {
                if (!string.IsNullOrEmpty(base64Image))
                {
                    bool pictureAdded = pictureService.AddPicture(base64Image);
                    if (!pictureAdded)
                    {
                        ShowErrorMessage("Failed to add the picture.");
                        return;
                    }

                    Picture addedPicture = pictureService.GetAllPictures().LastOrDefault();

                    Trip newTrip = new Trip
                    {
                        Name = name,
                        Description = description,
                        Accommodation = selectedAccomodation,
                        Picture = addedPicture
                    };

                    tripService.AddTrip(newTrip);

                    ClearForm();
                    navigateBackToHomePage?.Invoke();
                }
                else
                {
                    ShowErrorMessage("Please add an image.");
                }
            }
        }

        private bool ValidateForm(string name, string description, Accommodation selectedAccommodation)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || selectedAccommodation == null)
            {
                ShowErrorMessage("All fields are mandatory.");
                return false;
            }

            if (description.Length > 250)
            {
                ShowErrorMessage("Description should not exceed 250 characters.");
                return false;
            }

            return true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            navigateBackToHomePage?.Invoke();
        }

        private void ClearForm()
        {
            nameBox.Clear();
            descriptionBox.Clear();
            accommodationComboBox.SelectedItem = null;
            imagePreview.Source = null;
            base64Image = null;
        }

        private void ShowErrorMessage(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private void ShowSuccessMessage(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
