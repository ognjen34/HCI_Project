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

namespace HCI.Frames.Agemt
{
    /// <summary>
    /// Interaction logic for TripsStatisticPage.xaml
    /// </summary>
    public partial class TripsStatisticPage : Page
    {
        private IOrderedTripService _orderedTripService { get; }
        public TripsStatisticPage(IOrderedTripService tripService)
        {
            _orderedTripService = tripService;
            Dictionary<int, int> tripStatisticsMap = new Dictionary<int, int>();
            List<OrderedTrip> orderdTrips = _orderedTripService.GetAllOrderedTrips().ToList();
            InitializeComponent();
        }


    }

    
}
