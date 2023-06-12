using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Service;
using HCI.Models.Pictures.Model;
using HCI.Models.Pictures.Service;
using HCI.Models.Trips.Model;
using HCI.Models.Trips.Service;
using HCI.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Interaction logic for TripForm.xaml
    /// </summary>
    public partial class TripForm : UserControl
    {
        private readonly Trip existingTrip;
        private readonly ITripService tripService;
        private readonly IAccommodationService accommodationService;
        private readonly IPictureService pictureService;
        private readonly Action navigateBackToHomePage;
        private byte[] selectedImageBytes;

        public TripForm(Trip trip, ITripService tripService, IAccommodationService accommodationService, IPictureService pictureService, Action navigateBackToHomePage)
        {
            InitializeComponent();

            existingTrip = trip;
            this.tripService = tripService;
            this.accommodationService = accommodationService;
            this.pictureService = pictureService;
            this.navigateBackToHomePage = navigateBackToHomePage;

            PopulateAccommodations();

            if (existingTrip != null)
            {
                nameBox.Text = existingTrip.Name;
                descriptionBox.Text = existingTrip.Description;

                if (existingTrip.Picture != null)
                {
                    selectedImageBytes = Convert.FromBase64String(existingTrip.Picture.Pictures);
                    selectedImage.Source = LoadImage(selectedImageBytes);
                    imagePlaceholder.Visibility = Visibility.Collapsed;
                }

                saveButton.Content = "Update";
                formLabel.Text = "UPDATE TRIP";
            }

            cancelButton.Click += (sender, e) => navigateBackToHomePage?.Invoke();
            saveButton.Click += SaveButton_Click;

            imagePreview.Drop += ImagePreview_Drop;
            imagePreview.DragEnter += ImagePreview_DragEnter;
            imagePreview.DragLeave += ImagePreview_DragLeave;
            imagePreview.DragOver += ImagePreview_DragOver;
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
                    if (this.existingTrip != null)
                    {
                        if (this.existingTrip.Accommodation.Id == ((Accommodation)comboBoxItem.Tag).Id)
                        {
                            accommodationComboBox.SelectedItem = comboBoxItem;

                        }
                        else
                        {
                            continue;
                        }
                    }
                    else {
                        accommodationComboBox.SelectedItem = comboBoxItem;
                    }
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
            Accommodation selectedAccommodation = ((Accommodation)((ComboBoxItem)accommodationComboBox.SelectedItem).Tag);

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

            if (selectedImageBytes == null)
            {
                errorMessageTextBlock.Text = "Please drag and drop an image to the Picture box.";
                errorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            Trip trip = existingTrip ?? new Trip();
            trip.Name = name;
            trip.Description = description;
            trip.Accommodation = selectedAccommodation;

            if (selectedImageBytes != null)
            {
                if (existingTrip != null)
                {
                    Picture picture = new Picture { Pictures = Convert.ToBase64String(selectedImageBytes) };
                    trip.Picture = picture;
                    pictureService.DeletePicture(trip.Picture.Id);
                }
                else
                {
                    Picture picture = new Picture { Pictures = Convert.ToBase64String(selectedImageBytes) };
                    trip.Picture = picture;
                }
            }

            if (existingTrip != null)
            {
                tripService.UpdateTrip(trip);
            }
            else
            {
                tripService.AddTrip(trip);
            }

            navigateBackToHomePage?.Invoke();
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                HelpProvider.ShowHelp("agenttripsform", mainWindow,2);


            }
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
