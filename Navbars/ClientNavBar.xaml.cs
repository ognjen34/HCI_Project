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

namespace HCI.Navbars
{
    /// <summary>
    /// Interaction logic for ClientNavBar.xaml
    /// </summary>
    public partial class ClientNavBar : UserControl
    {
        public event EventHandler HomeClicked;
        public event EventHandler LogoutClicked;
        public event EventHandler HistoryClicked;

        public ClientNavBar()
        {
            InitializeComponent();
            homeButton.Click += HomeButton_Click;
            logoutButton.Click += LogoutButton_Click;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeClicked?.Invoke(this, EventArgs.Empty);
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            HelpProvider.ShowHelp("clienthome", mainWindow, 1);
        }


        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutClicked?.Invoke(this, EventArgs.Empty);
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("kara");
            HistoryClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
