using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
using System.Xml.Linq;

namespace dictionary_
{
    public partial class Search : Window
    {
        private string selectedWord;
        private string selectedCategory;
        private XmlHandler handler = new XmlHandler();
        public Search()
        {
            InitializeComponent();
            InitializeXmlData();
            InitializeUI();
        }

        private void InitializeXmlData()
        {
            DataContext = new
            {
                Words = handler.GetWords(),
                Categories = handler.GetCategories()
            };

            CategoryComboBox.ItemsSource = handler.GetCategories();
        }
       
        private void InitializeUI()
        {
            SearchBar.TextChanged += SearchBarTextChange; // triggers event when content is changed
            SearchBox.Click += SearchBoxClick; // triggers event when button is clicked
        }

        private void SearchBarTextChange(object sender, TextChangedEventArgs e)
        {
            string prefix = SearchBar.Text.ToLower();
            var suggestions = handler.GetWords();
            selectedCategory = ((Category)CategoryComboBox.SelectedItem)?.CategoryName;

            if (!string.IsNullOrEmpty(selectedCategory) && !selectedCategory.Equals("Default (all)", StringComparison.OrdinalIgnoreCase))
            {
                suggestions = suggestions
                    .Where(w => w.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var filteredSuggestions = suggestions
           .Where(w => w.WordValue.ToLower().StartsWith(prefix))
           .Select(w => w.WordValue)
           .ToList();

            SuggestionBox.ItemsSource = filteredSuggestions;

            if (filteredSuggestions.Any())
            {
                SuggestionBox.Visibility = Visibility.Visible;
            }
            else
            {
                SuggestionBox.Visibility = Visibility.Hidden;
            }
        }

        private void SuggestionBoxUpdate(object sender, SelectionChangedEventArgs e)
        {
            selectedWord = SuggestionBox.SelectedItem as String;

            if (!string.IsNullOrEmpty(selectedWord))
            {
                SearchBar.Text = selectedWord;
                SuggestionBox.Visibility = Visibility.Hidden;
            }
        }

        private void SearchBoxClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedWord))
            {
                var selectedWordDetails = handler.GetWords().FirstOrDefault
                    (w => w.WordValue.Equals(selectedWord, StringComparison.OrdinalIgnoreCase)); // case insensitive comparison

                if (selectedWordDetails != null)
                {
                    WordDetails.Text = $"Category: {selectedWordDetails.Category}\n\nDefinition: {selectedWordDetails.Definition}";
                    WordDetails.Visibility = Visibility.Visible;

                    WordPhoto.Source = new BitmapImage(new Uri(selectedWordDetails.ImagePath, UriKind.RelativeOrAbsolute));
                    WordPhoto.Visibility = Visibility.Visible;
                }
            }
        }

        private void UserGuessKeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                SearchBox.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
