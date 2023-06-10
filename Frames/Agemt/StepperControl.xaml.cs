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
    /// Interaction logic for StepperControl.xaml
    /// </summary>
    public partial class StepperControl : UserControl
    {
        public static readonly DependencyProperty StepNumberProperty =
            DependencyProperty.Register("StepNumber", typeof(int), typeof(StepperControl), new PropertyMetadata(OnStepNumberChanged));

        public static readonly DependencyProperty StepTextProperty =
            DependencyProperty.Register("StepText", typeof(string), typeof(StepperControl));

        public int StepNumber
        {
            get { return (int)GetValue(StepNumberProperty); }
            set { SetValue(StepNumberProperty, value); }
        }

        public string StepText
        {
            get { return (string)GetValue(StepTextProperty); }
            set { SetValue(StepTextProperty, value); }
        }

        public StepperControl()
        {
            InitializeComponent();
        }

        private static void OnStepNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StepperControl stepperControl = (StepperControl)d;
            stepperControl.UpdateStepVisuals();
        }

        private void UpdateStepVisuals()
        {
            if (StepNumber == 1)
            {
                firstStep.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                firstStepText.Visibility = Visibility.Collapsed;
            }
            else
            {
                firstStep.Background = new SolidColorBrush(Colors.White);
                firstStepText.Visibility = Visibility.Visible;
            }
        }
    }
}
