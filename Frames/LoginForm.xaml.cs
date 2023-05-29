using HCI.Models.Users.DTO;
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

namespace HCI
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : UserControl
    {
        private readonly IUserService _userService;
        public event EventHandler<LoginSuccessArgs> LoginSuccess;

        public LoginForm(IUserService userService)
        {
            InitializeComponent();

            _userService = userService;
            loginButton.Click += LoginButton_Click;
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the values from the form fields
            string username = usernameBox.Text;
            string password = passwordBox.Password;

            User user = _userService.GetUserByEmailAndPassword(username, password);
            if (user != null)
            {
                HideErrorMessage();

                // Raise the LoginSuccess event with user details
                OnLoginSuccess(user);
            }
            else
            {
                ShowErrorMessage("Incorrect login information.");
            }
        }

        protected virtual void OnLoginSuccess(User user)
        {
            // Check if there are any subscribers to the event
            LoginSuccess?.Invoke(this, new LoginSuccessArgs(user));
        }   

        private void ShowErrorMessage(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private void HideErrorMessage()
        {
            ErrorMessage.Text = "";
            ErrorMessage.Visibility = Visibility.Collapsed;
        }
    }
}