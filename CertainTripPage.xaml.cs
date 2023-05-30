using HCI.Models.Accommodations.Model;
using HCI.Models.Trips.Model;
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
    /// Interaction logic for CertainTripPage.xaml
    /// </summary>
    public partial class CertainTripPage : Page
    {
        public Trip Trip { get; set; }
        public int SelectedImg { get; set; }
        public List<Picture> Pictures { get; set; }
        public CertainTripPage(Trip trip)
        {
            Trip = trip;
            SelectedImg = 1;
            Pictures = new List<Picture>();
            Pictures = Trip.Accommodation.Pictures.ToList();
            InitializeComponent();
            Console.WriteLine(Pictures.Count);
            Pictures.ForEach(Picture => { Console.WriteLine(Picture.Pictures); });
            accomendationName.Text = trip.Accommodation.Name;
            accomendationDescritpion.Text = trip.Accommodation.Description;


            SetImageSource(Pictures[SelectedImg].Pictures);
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedImg == 0)
            {
                SelectedImg = Pictures.Count - 1;
                SetImageSource(Pictures[SelectedImg].Pictures);
            }
            else
            {
                SelectedImg = SelectedImg - 1; SetImageSource(Pictures[SelectedImg].Pictures);
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedImg == Pictures.Count - 1)
            {
                SelectedImg = 0;
                SetImageSource((Pictures[SelectedImg].Pictures));
            }
            else
            {
                SelectedImg = SelectedImg + 1; SetImageSource((Pictures[SelectedImg].Pictures));
            }

        }

        private void SetImageSource(string base64Image)
        {
            try
            {
                // Create a BitmapImage from the base64 string
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new System.IO.MemoryStream(Convert.FromBase64String(base64Image));
                bitmapImage.EndInit();

                // Set the BitmapImage as the source of the Image control
                accomendationImages.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error setting image source: " + ex.Message);
            }
        }
    }
}
