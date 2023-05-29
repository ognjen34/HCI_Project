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
using System.Windows.Shapes;

namespace HCI
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private readonly ITripService _userService;
        public ClientWindow(ITripService userService)
        {
            _userService = userService;
            InitializeComponent();
            
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomePage homePage = new HomePage(_userService); // Pass the service instance to the page constructor
            frameContent.Content = homePage;
        }
    }
}
