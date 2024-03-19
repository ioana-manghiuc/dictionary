using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using dictionary_;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace dictionary_
{
    public partial class Administrator : Window
    {
        private XmlHandler handler = new XmlHandler();
        int selectedWordId = -1;
        public Administrator()
        {
            InitializeComponent();
            InitializeXmlData();
            DisplayWords();
        }

        private void InitializeXmlData()
        {
            DataContext = new
            {
                Words = handler.GetWords(),
                Categories = handler.GetCategories()
            };
            var categoriesForAdding = handler.GetCategories().Where(c => !c.CategoryName.Equals("Default (all)", StringComparison.OrdinalIgnoreCase)).ToList();
            CategoryComboBox.ItemsSource = categoriesForAdding;
            WordsComboBox.ItemsSource = handler.GetWords();
            AllWordsComboBox.ItemsSource = handler.GetWords();
        }

        private void DisplayWords()
        {
            WordList.ItemsSource = handler.GetWords();
        }

        private void SeeWords(object sender, RoutedEventArgs e)
        {
            WordList.Visibility = Visibility.Visible;
            AddWordPanel.Visibility = Visibility.Collapsed;           
            ModifyWordPanel.Visibility = Visibility.Collapsed;
            DeleteWordPanel.Visibility = Visibility.Collapsed;
        }
        private void AddButton(object sender, RoutedEventArgs e)
        {
            AddWordPanel.Visibility = Visibility.Visible;
            WordList.Visibility = Visibility.Collapsed;
            ModifyWordPanel.Visibility = Visibility.Collapsed;
            DeleteWordPanel.Visibility = Visibility.Collapsed;
        }

        private void ModifyButton(object sender, RoutedEventArgs e)
        {
            ModifyWordPanel.Visibility = Visibility.Visible;
            WordList.Visibility = Visibility.Collapsed;
            AddWordPanel.Visibility = Visibility.Collapsed;
            DeleteWordPanel.Visibility = Visibility.Collapsed;
        }

        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            DeleteWordPanel.Visibility = Visibility.Visible;
            WordList.Visibility = Visibility.Collapsed;
            AddWordPanel.Visibility = Visibility.Collapsed;
            ModifyWordPanel.Visibility= Visibility.Collapsed;   
        }
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            AddWord();
        }

        private void ModifyButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateWord();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteWord();
        }

        private int GetNextWordId()
        {
            return handler.GetWords().Max(w => w.Id) + 1;
        }

        private void SearchImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Image Files",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string sourceFilePath = openFileDialog.FileName;
                string destinationFolder = "Resources";

                try
                {
                    Directory.CreateDirectory(destinationFolder);
                    string destinationFileName = System.IO.Path.GetFileName(sourceFilePath);
                    string destinationFilePath = System.IO.Path.Combine(destinationFolder, destinationFileName);
                    string destinationFullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destinationFilePath);
                    destinationFullPath = destinationFullPath.Replace("\\", "/");
                    File.Copy(sourceFilePath, destinationFullPath, true);
                    ImageText.Text = destinationFilePath.Replace("\\", "/");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error copying image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void AddWord()
        {
            try
            {
                Word newWord = new Word
                {
                    Id = GetNextWordId(),
                    WordValue = WordText.Text,
                    Definition = DefinitionText.Text,
                    ImagePath = ImageText.Text
                };

                handler.AddWordToXml(newWord, CategoryComboBox.Text.Trim(), "Resources/words.xml");
                InitializeXmlData();
                DisplayWords();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving word to XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WordsSelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWordValue = ((Word)AllWordsComboBox.SelectedItem)?.WordValue;

            if (!string.IsNullOrEmpty(selectedWordValue))
            {
                var selectedWordDetails = handler.GetWords().FirstOrDefault(
                    w => w.WordValue.Equals(selectedWordValue, StringComparison.OrdinalIgnoreCase));

                if (selectedWordDetails != null)
                {
                    selectedWordId = selectedWordDetails.Id;
                    CategoryBox.Text = selectedWordDetails.Category;
                    Definition.Text = selectedWordDetails.Definition;
                    ImageJPG.Text = selectedWordDetails.ImagePath;
                    SelectedWordDetails.Visibility = Visibility.Visible;
                    AllWordsComboBox.IsEditable = true;
                }
            }
        }

        private void UpdateWord()
        {
            try
            {
                var selectedWordValue = AllWordsComboBox.Text.Trim().ToString();

                if (!string.IsNullOrEmpty(selectedWordValue))
                {
                    handler.ModifyWordInXml(selectedWordId, selectedWordValue,
                        CategoryBox.Text.Trim(), Definition.Text.Trim(), ImageJPG.Text.Trim(),
                        "Resources/words.xml");

                    InitializeXmlData();
                    DisplayWords();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating word: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteWord()
        {
            try
            {
                handler.DeleteWordFromXml(WordsComboBox.Text.Trim(), "Resources/words.xml");
                InitializeXmlData();
                DisplayWords();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting word from XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
