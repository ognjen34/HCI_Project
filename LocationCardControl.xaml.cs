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
    /// Interaction logic for LocationCardControl.xaml
    /// </summary>
    public partial class LocationCardControl : UserControl
    {
        public Location Location { get; }
        public event EventHandler<EditLocationArgs> EditClickedEvent;
        public readonly ILocationService _locationService;

        public LocationCardControl(Location location, ILocationService locationService)
        {
            InitializeComponent();
            _locationService = locationService;
            Location = location;
            DataContext = Location;
            editButton.Click += EditClicked;
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            EditClickedEvent?.Invoke(this, new EditLocationArgs(this.Location));
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this location?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _locationService.DeleteLocation(Location.Id);

                var parentStackPanel = Parent as StackPanel;
                parentStackPanel?.Children.Remove(this);
            }
        }
    }
}
