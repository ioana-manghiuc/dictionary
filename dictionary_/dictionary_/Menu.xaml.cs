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

namespace dictionary_
{
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void OpenAdministrator(object sender, RoutedEventArgs e)
        {
            LogIn window1 = new LogIn();
            window1.Show();
            //Administrator window1 = new Administrator();
            //window1.Show();
        }

        private void OpenSearch(object sender, RoutedEventArgs e)
        {
            Search window2 = new Search();
            window2.Show();
        }

        private void OpenGame(object sender, RoutedEventArgs e)
        {
            GuessingGame window3 = new GuessingGame();
            window3.Show();
        }
    }
}
