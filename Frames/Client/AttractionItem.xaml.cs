using HCI.Models.Attractions.Model;
using HCI.Models.Restaurants.Model;
using HCI.Tools;
using System;
using System.IO;
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
        public Restaurant RestaurantData { get; set; }


        public AttractionItem(Attraction item = null,Restaurant res = null)
        {
            AttractionData = item;
            RestaurantData = res;
            InitializeComponent();

            MouseDown += AttractionItem_MouseDown;
            if( AttractionData != null) {
                Img.Source = StrToImg(AttractionData.Picture.Pictures);
                Price.Text = AttractionData.Price.ToString();
                Type.Text = AttractionData.ClassName;
                tripName.Text = AttractionData.Name;
                Location.Text = AttractionData.Location.Address;
            }
            if (RestaurantData != null)
            {
                Img.Source = StrToImg(RestaurantData.Picture.Pictures);
                Type.Text = RestaurantData.ClassName;
                tripName.Text = RestaurantData.Name;
                Location.Text = RestaurantData.Location.Address;
            }



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
