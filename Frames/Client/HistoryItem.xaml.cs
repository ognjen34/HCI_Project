using HCI.Models.Trips.Model;
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
using System.Windows.Shapes;

namespace HCI.Frames.Client
{
    /// <summary>
    /// Interaction logic for HistoryItem.xaml
    /// </summary>
    public partial class HistoryItem : UserControl
    {
        private OrderedTrip _trip;
        public HistoryItem(OrderedTrip trip)
        {
            _trip = trip;

            InitializeComponent();
            Img.Source = StrToImg(_trip.Trip.Picture.Pictures);
            Price.Text = _trip.TotalPrice.ToString();
            tripName.Text = _trip.Trip.Name;
            Location.Text = _trip.Trip.Accommodation.Location.Address;
            CheckIn.Text = _trip.CheckIn.ToString();
            CheckOut.Text = _trip.CheckOut.ToString();
            if (_trip.CheckIn > DateTime.Now)
            {
                TripStatus.Text = "Reserved";
                TripStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                TripStatus.Text = "Bought";
                TripStatus.Foreground = new SolidColorBrush(Colors.LightCoral);

            }
        }
        private void DetailsButtonClicked(object sender, EventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.contentControl.Navigate(new HistoryItemDetails(_trip));

        }




        private BitmapImage StrToImg(string base64Image)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                // Create a BitmapImage
                BitmapImage bitmapImage = new BitmapImage();

                // Set the BitmapImage's StreamSource to the MemoryStream
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                // Assign the BitmapImage as the source of the Image control
                return (bitmapImage);
            }
        }
    }

}
