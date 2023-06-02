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

namespace HCI.Navbars
{
    /// <summary>
    /// Interaction logic for AgentNavBar.xaml
    /// </summary>
    public partial class AgentNavBar : UserControl
    {
        public event EventHandler HomeClicked;
        public event EventHandler LocationClicked;
        public event EventHandler RestaurantClicked;
        public event EventHandler AttractionsClicked;
        public event EventHandler LogoutClicked;

        public AgentNavBar()
        {
            InitializeComponent();
            homeButton.Click += HomeButton_Click;
            logoutButton.Click += LogoutButton_Click;
            locationsButton.Click += LocationsButton_Click;
            restaurantsButton.Click += RestaurantsButton_Click;
            attractionsButton.Click += AttractionsButton_Click;

        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeClicked?.Invoke(this, EventArgs.Empty);
        }

        private void LocationsButton_Click(object sender, RoutedEventArgs e)
        {
            LocationClicked?.Invoke(this, EventArgs.Empty);
        }
        private void RestaurantsButton_Click(object sender, RoutedEventArgs e)
        {
            RestaurantClicked?.Invoke(this, EventArgs.Empty);
        }

        private void AttractionsButton_Click(object sender, RoutedEventArgs e)
        {
            AttractionsClicked?.Invoke(this, EventArgs.Empty);
        }


        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
