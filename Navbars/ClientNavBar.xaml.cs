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
    /// Interaction logic for ClientNavBar.xaml
    /// </summary>
    public partial class ClientNavBar : UserControl
    {
        public event EventHandler HomeClicked;
        public event EventHandler AboutClicked;
        public event EventHandler ContactClicked;
        public event EventHandler LogoutClicked;

        public ClientNavBar()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeClicked?.Invoke(this, EventArgs.Empty);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactClicked?.Invoke(this, EventArgs.Empty);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
