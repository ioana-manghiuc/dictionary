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

namespace dictionary_
{
    public partial class LogIn : Window
    {
        private AdminXmlHandler handler = new AdminXmlHandler();
        public LogIn()
        {
            InitializeComponent();
            InitializeXmlData();
        }

        private void InitializeXmlData()
        {
            DataContext = new
            {
                Admins = handler.GetAdmins()
            };
        }

        private void UserGuessKeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                LoginButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            RegisterPanel.Visibility = Visibility.Collapsed;
            Admin enteredAdmin = new Admin {
                Email = EmailText.Text,
                Password = PasswordText.Password
            };

            if(handler.FindAdmin(enteredAdmin) == true)
            {
                Administrator window = new Administrator();
                window.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Error - admin not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }         
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            RegisterPanel.Visibility= Visibility.Visible;
            LoginPanel.Visibility = Visibility.Collapsed;
        
        }
        private int GetNextAdminId()
        {
            return handler.GetAdmins().Max(a => a.Id) + 1;
        }

        public void Register()
        {
            try
            {

                DateTime bd = new DateTime(2005, 12, 31);
                if (BirthdayText.SelectedDate.Value > bd)
                {
                    MessageBox.Show($"Invalid date of birth!\nPlease select before: 31-Jan-2005.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Admin newAdmin = new Admin
                {
                    Id = GetNextAdminId(),
                    GivenName = GivenNameText.Text,
                    FamilyName = FamilyNameText.Text,
                    DateOfBirth = BirthdayText.Text,
                    Email = EmailTextbox.Text,
                    Password = PasswordTextbox.Text,
                };
                
                handler.AddAdminToXml(newAdmin, "Resources/admins.xml");
                InitializeXmlData();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterInXmlClick(object sender, RoutedEventArgs e)
        {
            RegisterPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            Register();
        }

    }
}
