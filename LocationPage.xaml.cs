using HCI.Models.Locations.DTO;
using HCI.Models.Locations.Model;
using HCI.Models.Locations.Service;
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
    /// Interaction logic for LocationPage.xaml
    /// </summary>
    public partial class LocationPage : Page
    {
        private readonly ILocationService _locationService;

        public LocationPage(ILocationService locationService)
        {
            this._locationService = locationService;
            InitializeComponent();
            List<Location> locations = _locationService.GetAllLocations();
            Console.WriteLine(locations.Count);
            foreach (Location location in locations)
            {
                LocationCardControl locationCard = new LocationCardControl(location,_locationService);
                locationsStackPanel.Children.Add(locationCard);
                locationCard.EditClickedEvent += LocationCardControl_EditClicked;
            }
        }

        private void LocationCardControl_EditClicked(object sender, EditLocationArgs e)
        {
            Location location = e.location;
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new LocationForm(location,_locationService, () => mainWindow.NavigateToLocationsAgent()));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new LocationForm(_locationService, () => mainWindow.NavigateToLocationsAgent()));
        }

    }
}
