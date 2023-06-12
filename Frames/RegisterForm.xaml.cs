using HCI.Models.Users.Model;
using HCI.Models.Users.Service;
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
            string name = nameBox.Text.Trim();
            string surname = surnameBox.Text.Trim();
            string username = usernameBox.Text.Trim();
            string password = passwordBox.Password.Trim();
            string confirmPassword = confirmPasswordBox.Password.Trim();

            ClearErrorMessages();

            if (string.IsNullOrEmpty(name))
            {
                ShowErrorMessage("Fill in the Name field.");
                return;
            }

            if (string.IsNullOrEmpty(surname))
            {
                ShowErrorMessage("Fill in the Surname field.");
                return;
            }

            if (string.IsNullOrEmpty(username))
            {
                ShowErrorMessage("Fill in the Username field.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowErrorMessage("Fill in the Password field.");
                return;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                ShowErrorMessage("Fill in the Confirm Password field.");
                return;
            }

            if (name.Length > 30)
            {
                ShowErrorMessage("Name cannot exceed 30 characters!");
                return;
            }

            if (surname.Length > 30)
            {
                ShowErrorMessage("Surname cannot exceed 30 characters!");
                return;
            }

            if (!IsValidEmail(username))
            {
                ShowErrorMessage("Username needs to be in email format!");
                return;
            }

            if (password.Length < 8)
            {
                ShowErrorMessage("Password must be at least 8 characters long!");
                return;
            }

            if (password != confirmPassword)
            {
                ShowErrorMessage("Password and Confirm Password do not match!");
                return;
            }
            User doesAlreadyExist  = _userService.GetUserByEmail(username);
            if (doesAlreadyExist != null)
            {
                ShowErrorMessage("User with this email already exists!");
                return;
            }

            User user = new User();
            user.Name = name;
            user.Surname = surname;
            user.Password = password;
            user.Email = username;
            _userService.AddUser(user);

            ShowSuccessMessage("Registration successful!");
            buttonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            buttonClicked?.Invoke(this, EventArgs.Empty);
        }


        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                HelpProvider.ShowHelp("register", mainWindow, 0);
            }
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

        private void ClearErrorMessages()
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
