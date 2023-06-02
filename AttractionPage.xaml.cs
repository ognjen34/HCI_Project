using HCI.Models.Attractions.DTO;
using HCI.Models.Attractions.Model;
using HCI.Models.Attractions.Service;
using HCI.Models.Locations.Service;
using HCI.Models.Pictures.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HCI
{
    /// <summary>
    /// Interaction logic for AttractionPage.xaml
    /// </summary>
    public partial class AttractionPage : Page
    {
        private readonly IAttractionService _attractionService;
        private readonly ILocationService _locationService;
        private readonly IPictureService _pictureService;


        public AttractionPage(IAttractionService attractionService,ILocationService locationService,IPictureService pictureService)
        {
            _attractionService = attractionService;
            _locationService = locationService;
            _pictureService = pictureService;


            InitializeComponent();
            IEnumerable<Attraction> attractions = _attractionService.GetAll();
            foreach (Attraction attraction in attractions)
            {
                AttractionCardControl attractionCard = new AttractionCardControl(attraction, _attractionService);
                attractionsStackPanel.Children.Add(attractionCard);
                attractionCard.EditClickedEvent += AttractionCardControl_EditClicked;
            }
        }

        private void AttractionCardControl_EditClicked(object sender, EditAttracitonArgs e)
        {
            Attraction attraction = e.attraction;
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new AttractionForm(attraction, _locationService, _pictureService, _attractionService, () => mainWindow.NavigateToAttractionsAgent()));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentControl.Navigate(new AttractionForm(null, _locationService, _pictureService, _attractionService, () => mainWindow.NavigateToAttractionsAgent()));
        }
    }
}
