using HCI.Models.Attractions.Model;
using HCI.Models.Restaurants.Model;
using HCI.Models.Trips.Model;
using HCI.Models.Trips.Service;
using HCI.Models.Users.Model;
using HCI.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace HCI.Frames.Client
{
    /// <summary>
    /// Interaction logic for AttractionItem.xaml
    /// </summary>
    public partial class AttractionItem : UserControl
    {
        private bool isClicked = false;

        public event EventHandler ItemClicked;

        public Attraction AttractionData { get; set; }
        public IOrderedTripService _orderedTripService;
        public Restaurant RestaurantData { get; set; }
        public User _user;
        private Dictionary<int, int> _attractionStatistic;
        private Trip _trip;

        public AttractionItem(Attraction item = null,Restaurant res = null, User user = null, IOrderedTripService orderedTripService = null, Trip trip = null)
        {

            AttractionData = item;
            _trip = trip;
            _user = user;
            RestaurantData = res;
            _orderedTripService = orderedTripService;
            InitializeComponent();
            _attractionStatistic = getAttractionStatistics();
            MouseDown += AttractionItem_MouseDown;
            if(user.Type == UserType.Agent)
            {
                statistics.Visibility = Visibility.Visible;
            }
            else
            {
                statistics.Visibility = Visibility.Collapsed;
            }
            if( AttractionData != null) {
                Img.Source = StrToImg(AttractionData.Picture.Pictures);
                Price.Text = AttractionData.Price.ToString();
                Type.Text = AttractionData.ClassName;
                tripName.Text = AttractionData.Name;
                Location.Text = AttractionData.Location.Address;
                statistics.Text = "Sold: " + _attractionStatistic[AttractionData.Id].ToString();
            }
            if (RestaurantData != null)
            {
                Img.Source = StrToImg(RestaurantData.Picture.Pictures);
                Type.Text = RestaurantData.ClassName;
                tripName.Text = RestaurantData.Name;
                Location.Text = RestaurantData.Location.Address;
                statistics.Text = "Sold: " + _attractionStatistic[RestaurantData.Id].ToString();
            }



        }

        private Dictionary<int, int> getAttractionStatistics()
        {
            Dictionary<int, int> attractionStatistic = new Dictionary<int, int>();
            List<OrderedTrip> orderedTrips = _orderedTripService.GetAllOrderedTrips().ToList();
            if (RestaurantData == null)
            {
               foreach(OrderedTrip orderedTrip in orderedTrips)
                {
                    if(orderedTrip.Trip.Id == _trip.Id)
                    {
                        foreach (Attraction attraction in orderedTrip.Attractions)
                        {
                            if (attractionStatistic.ContainsKey(attraction.Id))
                            {
                                attractionStatistic[attraction.Id] += 1;
                            }
                            else
                            {
                                attractionStatistic.Add(attraction.Id, 1);
                            }
                        }
                    }
                    
                }
            }
            else
            {
                foreach (OrderedTrip orderedTrip in orderedTrips)
                {
                    if (orderedTrip.Trip.Id == _trip.Id)
                    {
                        foreach (Restaurant attraction in orderedTrip.Restaurants)
                        {
                            if (attractionStatistic.ContainsKey(attraction.Id))
                            {
                                attractionStatistic[attraction.Id] += 1;
                            }
                            else
                            {
                                attractionStatistic.Add(attraction.Id, 1);
                            }
                        }
                    }

                }

            }
            return attractionStatistic;
        }

        private void AttractionItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ToggleGlowEffect();
            ItemClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ToggleGlowEffect()
        {
            if (isClicked)
                RemoveGlowEffect();
            else
                ApplyGlowEffect();

            isClicked = !isClicked;
        }

        private void ApplyGlowEffect()
        {
            var dropShadowEffect = new DropShadowEffect
            {
                Color = Colors.Blue,
                Direction = 0,
                ShadowDepth = 0,
                BlurRadius = 20,
                Opacity = 1
            };

            Border.Effect = dropShadowEffect;
        }

        public void RemoveGlowEffect()
        {
            var dropShadowEffect = new DropShadowEffect
            {
                Color = Colors.Gray,
                Direction = 0,
                ShadowDepth = 0,
                BlurRadius = 10,
                Opacity = 0.5
            };

            Border.Effect = dropShadowEffect;
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
