using HCI.Models.Accommodations.DTO;
using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Service;
using HCI.Models.Locations.Model;
using HCI.Models.Pictures.Model;
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

namespace HCI.Frames.Agemt
{
    /// <summary>
    /// Interaction logic for AccommodationCardControl.xaml
    /// </summary>
    public partial class AccommodationCardControl : UserControl
    {
        public Accommodation Accommodation { get; }
        public event EventHandler<EditAccomodationArgs> EditClickedEvent;
        private readonly IAccommodationService _accommodationService;

        public AccommodationCardControl(Accommodation accommodation, IAccommodationService accommodationService)
        {
            InitializeComponent();
            _accommodationService = accommodationService;
            Accommodation = accommodation;
            DataContext = Accommodation;
            editButton.Click += EditClicked;
            LoadBase64Image();
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            EditClickedEvent?.Invoke(this, new EditAccomodationArgs(this.Accommodation));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this accommodation? This action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _accommodationService.RemoveAccommodation(this.Accommodation);

                var parentStackPanel = Parent as StackPanel;
                parentStackPanel?.Children.Remove(this);
            }
        }

        private void LoadBase64Image()
        {
            if (Accommodation.Pictures != null && Accommodation.Pictures.Any())
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(Accommodation.Pictures[0].Pictures);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new System.IO.MemoryStream(imageBytes);
                    bitmap.EndInit();
                    accommodationImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading image: " + ex.Message);
                }
            }
        }
    }
}
