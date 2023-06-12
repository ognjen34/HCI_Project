using HCI.Models.Trips.DTO;
using HCI.Models.Trips.Model;
using HCI.Models.Trips.Service;
using HCI.Models.Users.Model;
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
    /// Interaction logic for TripCardControl.xaml
    /// </summary>
    public partial class TripCardControl : UserControl
    {
        public Trip Trip;
        public event EventHandler<OrderTripArgs> OrderTrip;
        public event EventHandler<OrderTripArgs> EditClickedEvent;
        public event EventHandler<OrderTripArgs> DetailsClickedEvent;
        public event EventHandler<OrderTripArgs> DeleteClickedEvent;
 

        private readonly UserType userType;
        private readonly ITripService _tripService;
        private readonly IOrderedTripService _orderedTripService;
        private Dictionary<int, int> _tripStatistics;
        private Dictionary<int, double> _tripTotalPrice;

        public TripCardControl(Trip trip, UserType userType, ITripService tripService, IOrderedTripService orderedTripService)
        {
            this.userType = userType;
            _tripService = tripService;
            this._orderedTripService = orderedTripService;
            Trip = trip;
            InitializeComponent();
            tripName.Text = trip.Name;
            description.Text = trip.Description;
            bedsValue.Text = trip.Accommodation.Beds.ToString();
            locationName.Text = trip.Accommodation.Location.Address.ToString();
            if (!trip.IsDeleted)
            {
                isActiveValue.Text = "Active";
            }
            else
            {
                isActiveValue.Text = "Inactive";
            }
            if(this.userType == UserType.Agent)
            {
                _tripStatistics = getTripStatistics();
                _tripTotalPrice = getTotalPrice();
                statisticsValue.Text = _tripStatistics[trip.Id].ToString();
                totalPriceValue.Text = _tripTotalPrice[trip.Id].ToString() + " $";
            }
            
            Console.WriteLine(trip.Name);
            Console.WriteLine(trip.Description);
            SetImageSource(trip.Picture.Pictures);
            accomendationPrice.Text = Trip.Accommodation.PricePerDay.ToString() + "$/Night";
            UpdateButtonVisibility();
        }

        private void UpdateButtonVisibility()
        {
            switch (userType)
            {
                case UserType.Agent:
                    orderButton.Visibility = Visibility.Collapsed;
                    editButton.Visibility = Visibility.Visible;
                    statistics.Visibility = Visibility.Visible;
                    totalPrice.Visibility = Visibility.Visible;
                    totalPriceValue.Visibility = Visibility.Visible;
                    statisticsValue.Visibility = Visibility.Visible;
                    isActive.Visibility = Visibility.Visible;
                    isActiveValue.Visibility = Visibility.Visible;
                    detailButton.Visibility = Visibility.Visible;
                    deleteButton.Visibility = Visibility.Visible;
                    break;
                case UserType.Client:
                    orderButton.Visibility = Visibility.Visible;
                    statistics.Visibility = Visibility.Collapsed;
                    statisticsValue.Visibility = Visibility.Collapsed;
                    totalPrice.Visibility = Visibility.Collapsed;
                    totalPriceValue.Visibility = Visibility.Collapsed;
                    isActive.Visibility = Visibility.Collapsed;
                    isActiveValue.Visibility = Visibility.Collapsed;
                    detailButton.Visibility = Visibility.Collapsed;
                    editButton.Visibility = Visibility.Collapsed;
                    deleteButton.Visibility = Visibility.Collapsed;
                    break;
            }

            if(Trip.IsDeleted == true)
            {
                editButton.Visibility = Visibility.Collapsed;
                deleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private Dictionary<int, int> getTripStatistics()
        {
            Dictionary<int, int> tripStatisticsMap = new Dictionary<int, int>();
            Dictionary<int, int> tripPriceMap = new Dictionary<int, int>();
            List<OrderedTrip> orderedTrips = _orderedTripService.GetAllOrderedTrips().ToList();
            foreach (OrderedTrip trip in orderedTrips)
            {
                if (tripStatisticsMap.ContainsKey(trip.Trip.Id))
                {
                    tripStatisticsMap[trip.Trip.Id] += 1;
                }
                else
                {
                    tripStatisticsMap.Add(trip.Trip.Id, 1);
                }
            }
            return tripStatisticsMap;

        }

        private Dictionary<int, double> getTotalPrice()
        {
            Dictionary<int, double> tripPriceMap = new Dictionary<int, double>();
            List<OrderedTrip> orderedTrips = _orderedTripService.GetAllOrderedTrips().ToList();
            foreach (OrderedTrip trip in orderedTrips)
            {
                if (tripPriceMap.ContainsKey(trip.Trip.Id))
                {
                    tripPriceMap[trip.Trip.Id] += trip.TotalPrice;
                }
                else
                {
                    tripPriceMap.Add(trip.Trip.Id, trip.TotalPrice);
                }
            }
            return tripPriceMap;

        }

        private void SetImageSource(string base64Image)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new System.IO.MemoryStream(Convert.FromBase64String(base64Image));
                bitmapImage.EndInit();

                imageControl.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error setting image source: " + ex.Message);
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderTrip?.Invoke(this, new OrderTripArgs(Trip));
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            EditClickedEvent?.Invoke(this, new OrderTripArgs(this.Trip));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this trip? This action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _tripService.RemoveTrip(Trip);

                DeleteClickedEvent?.Invoke(this, new OrderTripArgs(this.Trip));
            }
        }


        private void detailButton_Click(object sender, RoutedEventArgs e)
        {
            DetailsClickedEvent?.Invoke(this, new OrderTripArgs(this.Trip));
        }
    }
}
