using HCI.Models.Attractions.DTO;
using HCI.Models.Attractions.Model;
using HCI.Models.Attractions.Service;
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
    /// Interaction logic for AttractionCardControl.xaml
    /// </summary>
    public partial class AttractionCardControl : UserControl
    {
        public Attraction Attraction { get; }
        public event EventHandler<EditAttracitonArgs> EditClickedEvent;
        private readonly IAttractionService _attractionService;

        public AttractionCardControl(Attraction attraction, IAttractionService attractionService)
        {
            InitializeComponent();
            _attractionService = attractionService;
            Attraction = attraction;
            DataContext = Attraction;
            editButton.Click += EditClicked;
            LoadBase64Image();
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            EditClickedEvent?.Invoke(this, new EditAttracitonArgs(this.Attraction));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this attraction?,this action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _attractionService.Delete(this.Attraction.Id);

                var parentStackPanel = Parent as StackPanel;
                parentStackPanel?.Children.Remove(this);
            }
        }

        private void LoadBase64Image()
        {
            if (Attraction.Picture != null)
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(Attraction.Picture.Pictures);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new System.IO.MemoryStream(imageBytes);
                    bitmap.EndInit();
                    attractionImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading image: " + ex.Message);
                }
            }
        }
    }
}
