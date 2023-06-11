using HCI.Models.Accommodations.DTO;
using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Service;
using HCI.Models.Attractions.DTO;
using HCI.Models.Attractions.Model;
using HCI.Models.Locations.Service;
using HCI.Models.Pictures.Service;
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

namespace HCI.Frames.Agemt
{
    /// <summary>
    /// Interaction logic for AccomodationPage.xaml
    /// </summary>
    public partial class AccomodationPage : Page
    {
        private ILocationService locationService;
        private IPictureService pictureService;
        private IAccommodationService accommodationService;

        

        public AccomodationPage(IAccommodationService _accomodationService, ILocationService locationService, IPictureService pictureService) 
        {
            this.locationService = locationService;
            this.pictureService = pictureService;
            this.accommodationService = _accomodationService;
            InitializeComponent();
            IEnumerable<Accommodation> accomodations = accommodationService.GetAllAccommodations();
            foreach (Accommodation accomodation in accomodations)
            {
                AccommodationCardControl accomodationCard = new AccommodationCardControl(accomodation, accommodationService);
                accomodationsStackPanel.Children.Add(accomodationCard);
                accomodationCard.EditClickedEvent += AccomodationCardEdit_Clicked;
            }
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                HelpProvider.ShowHelp("agentaccommodations", mainWindow, 2);

            }

        }
        private void AccomodationCardEdit_Clicked(object? sender, EditAccomodationArgs e)
        {
            Accommodation accomodaton = e.accomodation;
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new AccomodationForm(accomodaton, accommodationService, locationService, pictureService, () => mainWindow.NavigateToAccomodationsAgent()));
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new AccomodationForm(null, accommodationService, locationService, pictureService, () => mainWindow.NavigateToAccomodationsAgent()));

        }
      
    }
}
