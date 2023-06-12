using HCI.Models.Attractions.Model;
using HCI.Models.Trips.Model;
using HCI.Models.Trips.Repository;
using HCI.Models.Trips.Service;
using HCI.Models.Users.Model;
using HCI.Tools;
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

namespace HCI.Frames.Client
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : UserControl
    {
        private IOrderedTripService _orderedTripService;
        private User _user;
        public History(IOrderedTripService orderedTripService, User user)
        {
            _orderedTripService = orderedTripService;
            _user = user;
            InitializeComponent();
            List<OrderedTrip> trips = (List<OrderedTrip>)_orderedTripService.GetAllByUserId(_user.Id);
            foreach (var item in trips)
            {
                var historyItem = new HistoryItem(item, _user, _orderedTripService);
                //historyItem.ItemClicked += AttractionItem_Clicked;
                historyItemsControl.Items.Add(historyItem);
            }
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if (_user.Type == UserType.Client)
                {
                    HelpProvider.ShowHelp("clienthistory", mainWindow,1);
                }
                
            }
        }

    }
}
