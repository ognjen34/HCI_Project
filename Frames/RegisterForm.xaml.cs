using HCI.Models.Users.Model;
using HCI.Models.Users.Service;
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

namespace HCI.Frames
{
    /// <summary>
    /// Interaction logic for RegisterForm.xaml
    /// </summary>
    public partial class RegisterForm : UserControl
    {
        private readonly IUserService _userService;
        public event EventHandler buttonClicked;

        public RegisterForm(IUserService userService)
        {
            _userService = userService;
            InitializeComponent();
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            
            if(nameBox.Text.Trim() == "" || surnameBox.Text.Trim() == "" || passwordBox.Password.Trim() == "" || usernameBox.Text.Trim() == "")
            {
                ShowErrorMessage("You must fill every field!");
            }

            else if(_userService.GetUserByEmail(usernameBox.Text) != null)
            {
                
                
                ShowErrorMessage("Email is already in use!");
            }

            else
            {
                User user = new User();
                user.Name = nameBox.Text;
                user.Surname = surnameBox.Text;
                user.Password = passwordBox.Password;
                user.Email = usernameBox.Text;
                _userService.AddUser(user);

                ShowSuccessMessage("Registration successful!");
                buttonClicked?.Invoke(this, EventArgs.Empty);
            }
            
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            buttonClicked?.Invoke(this, EventArgs.Empty);

        }

        private void ShowErrorMessage(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
