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
    }
}
