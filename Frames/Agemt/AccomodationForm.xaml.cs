using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Service;
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

namespace HCI.Frames.Agemt
{
    /// <summary>
    /// Interaction logic for RestaurantForm.xaml
    /// </summary>
    public partial class AccomodationForm : UserControl
    {
        private readonly Accommodation existingAccomodation;
        private readonly IAccommodationService accomodationService;
        private readonly ILocationService locationService;
        private readonly IPictureService pictureService;
        private readonly Action navigateBackToRestaurants;
        private List<byte[]> selectedPictures;
        private bool drop = false;

        public AccomodationForm(Accommodation accommodation, IAccommodationService accomodationService, ILocationService locationService, IPictureService pictureService, Action navigateBackToRestaurants)
        {
            InitializeComponent();
            selectedPictures = new List<byte[]>();
            for (int i = 0; i < 7; i++)
            {
                selectedPictures.Add(new byte[0]);
            }
            existingAccomodation = accommodation;
            this.accomodationService = accomodationService;
            this.locationService = locationService;
            this.pictureService = pictureService;
            this.navigateBackToRestaurants = navigateBackToRestaurants;


       

            if (existingAccomodation != null)
            {
                nameBox.Text = existingAccomodation.Name;
                descriptionBox.Text = existingAccomodation.Description;
                priceBox.Text = existingAccomodation.PricePerDay.ToString();
                bedsBox.Text = existingAccomodation.Beds.ToString();

                 if(existingAccomodation.Pictures.Count != 0)
                {
                    selectedPictures[0] = Convert.FromBase64String(existingAccomodation.Pictures[0].Pictures);
                    selectedImage.Source = LoadImage(selectedPictures[0]);
                    imagePlaceholder.Visibility = Visibility.Collapsed;
                    AddPictures();
                }

                formLabel.Text = "UPDATE ACCOMMODATION";
            }

            cancelButton.Click += (sender, e) => navigateBackToRestaurants?.Invoke();
            nextButton.Click += NextButton_Click;

            imagePreview.Drop += ImagePreview_Drop;
            imagePreview.DragEnter += ImagePreview_DragEnter;
            imagePreview.DragLeave += ImagePreview_DragLeave;
            imagePreview.DragOver += ImagePreview_DragOver;
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                HelpProvider.ShowHelp("agentaccommodationsform", mainWindow);


            }
        }

        private void AddPictures()
        {
            if (existingAccomodation.Pictures.Count>=2)
            {
                selectedPictures[1] = Convert.FromBase64String(existingAccomodation.Pictures[1].Pictures);
                ImgOne.Source = LoadImage(selectedPictures[1]);
            }
            if (existingAccomodation.Pictures.Count >= 3)
            {
                selectedPictures[2] = Convert.FromBase64String(existingAccomodation.Pictures[2].Pictures);
                ImgTwo.Source = LoadImage(selectedPictures[2]);
            }
            if (existingAccomodation.Pictures.Count >= 4)
            {
                selectedPictures[3] = Convert.FromBase64String(existingAccomodation.Pictures[3].Pictures);
                ImgThree.Source = LoadImage(selectedPictures[3]);
            }
            if (existingAccomodation.Pictures.Count >= 5)
            {
                selectedPictures[4] = Convert.FromBase64String(existingAccomodation.Pictures[4].Pictures);
                ImgFour.Source = LoadImage(selectedPictures[4]);
            }
            if (existingAccomodation.Pictures.Count ==6)
            {
                selectedPictures[5] = Convert.FromBase64String(existingAccomodation.Pictures[5].Pictures);
                ImgFive.Source = LoadImage(selectedPictures[5]);
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
        private void ImageOne_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (selectedPictures[2].Length > 1)
            {
                selectedPictures[1] = selectedPictures[2];
                ImgOne.Source = LoadImage(selectedPictures[1]);
            }
            else
            {
                selectedPictures[1] = new byte[0];
                ImgOne.Source = null;
            }
            if (selectedPictures[3].Length > 1)
            {
                selectedPictures[2] = selectedPictures[3];
                ImgTwo.Source = LoadImage(selectedPictures[2]);
            }
            else
            {
                selectedPictures[2] = new byte[0];
                ImgTwo.Source = null;
            }
            if (selectedPictures[4].Length > 1)
            {
                selectedPictures[3] = selectedPictures[4];
                ImgThree.Source = LoadImage(selectedPictures[3]);
            }
            else
            {
                selectedPictures[3] = new byte[0];
                ImgThree.Source = null;
            }
            if (selectedPictures[5].Length > 1)
            {
                selectedPictures[4] = selectedPictures[5];
                ImgFour.Source = LoadImage(selectedPictures[4]);
            }
            else
            {
                selectedPictures[4] = new byte[0];
                ImgFour.Source = null;
            }
            selectedPictures[5] = new byte[0];
            ImgFive.Source = null;

        }
        private void ImageTwo_Clicked(object sender, MouseButtonEventArgs e)
        {

            if (selectedPictures[3].Length > 1)
            {
                selectedPictures[2] = selectedPictures[3];
                ImgTwo.Source = LoadImage(selectedPictures[2]);
            }
            else
            {
                selectedPictures[2] = new byte[0];
                ImgTwo.Source = null;
            }
            if (selectedPictures[4].Length > 1)
            {
                selectedPictures[3] = selectedPictures[4];
                ImgThree.Source = LoadImage(selectedPictures[3]);
            }
            else
            {
                selectedPictures[3] = new byte[0];
                ImgThree.Source = null;
            }
            if (selectedPictures[5].Length > 1)
            {
                selectedPictures[4] = selectedPictures[5];
                ImgFour.Source = LoadImage(selectedPictures[4]);
            }
            else
            {
                selectedPictures[4] = new byte[0];
                ImgFour.Source = null;
            }
            selectedPictures[5] = new byte[0];
            ImgFive.Source = null;

        }
        private void ImageThree_Clicked(object sender, MouseButtonEventArgs e)
        {


            if (selectedPictures[4].Length > 1)
            {
                selectedPictures[3] = selectedPictures[4];
                ImgThree.Source = LoadImage(selectedPictures[3]);
            }
            else
            {
                selectedPictures[3] = new byte[0];
                ImgThree.Source = null;
            }
            if (selectedPictures[5].Length > 1)
            {
                selectedPictures[4] = selectedPictures[5];
                ImgFour.Source = LoadImage(selectedPictures[4]);
            }
            else
            {
                selectedPictures[4] = new byte[0];
                ImgFour.Source = null;
            }
            selectedPictures[5] = new byte[0];
            ImgFive.Source = null;

        }
        private void ImageFour_Clicked(object sender, MouseButtonEventArgs e)
        {

            if (selectedPictures[5].Length > 1)
            {
                selectedPictures[4] = selectedPictures[5];
                ImgFour.Source = LoadImage(selectedPictures[4]);
            }
            else
            {
                selectedPictures[4] = new byte[0];
                ImgFour.Source = null;
            }
            selectedPictures[5] = new byte[0];
            ImgFive.Source = null;

        }
        private void ImageFive_Clicked(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("XDDD");
            selectedPictures[5] = new byte[0];
            ImgFive.Source = null;

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text.Trim();
            string description = descriptionBox.Text.Trim();
            string price = priceBox.Text.Trim();
            string beds = bedsBox.Text.Trim();

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

           

            if (selectedPictures[0] == null)
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

            

            Accommodation accomodation = existingAccomodation ?? new Accommodation();
            accomodation.Name = name;
            accomodation.Description = description;
            accomodation.PricePerDay = Double.Parse(price);
            accomodation.Beds = int.Parse(beds);

            if (selectedPictures.Count != 0)
            {
               
                    accomodation.Pictures = new List<Picture>();
                    foreach (byte[] b in selectedPictures)
                    { 
                        if (b.Length > 1)
                        {
                            Picture picture = new Picture { Pictures = Convert.ToBase64String(b) };
                            accomodation.Pictures.Add(picture);
                        }
                    }
                    
                    
                
                
            }

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new LocationForm(accomodation, locationService, accomodationService, () => mainWindow.contentControl.Navigate(new AccomodationForm(accomodation, accomodationService, locationService, pictureService, () => mainWindow.NavigateToAccomodationsAgent())), () => mainWindow.NavigateToAccomodationsAgent()));
        }



        private void ImagePreview_Drop(object sender, DragEventArgs e)
        {

            if (!drop)
            {
                drop = true;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                        MovePictures();
                        selectedPictures[0] = File.ReadAllBytes(files[0]);
                        selectedImage.Source = LoadImage(selectedPictures[0]);
                        imagePlaceholder.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else 
            {
                drop = false;
            }
        }

        private void MovePictures()
        {
            
            selectedPictures[5] = selectedPictures[4];
            if (selectedPictures[5].Length >1)
                ImgFive.Source = LoadImage(selectedPictures[5]);
            selectedPictures[4] = selectedPictures[3];
            if (selectedPictures[4].Length > 1)
                ImgFour.Source = LoadImage(selectedPictures[4]);
            selectedPictures[3] = selectedPictures[2];
            if (selectedPictures[3].Length > 1)
                ImgThree.Source = LoadImage(selectedPictures[3]);
            selectedPictures[2] = selectedPictures[1];
            if (selectedPictures[2].Length > 1)
                ImgTwo.Source = LoadImage(selectedPictures[2]);
            selectedPictures[1] = selectedPictures[0];
            if (selectedPictures[1].Length > 1)
                ImgOne.Source = LoadImage(selectedPictures[1]);

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
