using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace dictionary_
{
    [Serializable]
    internal class XmlHandler
    {
        [XmlArray]
        private List<Word> words {  get; set; }

        [XmlArray]
        private List<Category> categories { get; set; } 

        public XmlHandler() 
        {
            LoadData();
        }

        private void LoadData()
        {
            words = GetWordsFromXml("Resources/words.xml");
            categories = GetCategoriesFromXml("Resources/categories.xml");
        }

        private List<Category> GetCategoriesFromXml(string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);
                return doc.Root.Elements("Category")
                    .Select(c => new Category
                    {
                        Id = int.Parse(c.Element("Id").Value),
                        CategoryName = c.Element("CategoryName").Value
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories from XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Category>();
            }
        }

        private List<Word> GetWordsFromXml(string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                return doc.Root.Elements("Word")
                    .Select(w => new Word
                    {
                        Id = int.Parse(w.Element("Id").Value),
                        WordValue = w.Element("WordValue").Value,
                        Category = w.Element("Category").Value,
                        Definition = w.Element("Definition").Value,
                        ImagePath = w.Element("ImagePath").Value
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading words from XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Word>();
            }
        }

        public List<Word> GetWords() { return words; }

        public List<Category> GetCategories() { return categories; }

        public void AddWordToXml(Word newWord, string enteredCategory, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);
                XDocument docCategs = XDocument.Load("Resources/categories.xml");

                string path;
                if (newWord.ImagePath == string.Empty)
                {
                    path = "Resources/000.jpg";
                }
                else
                {
                    path = "Resources/" + newWord.ImagePath;
                }

                XElement newWordElement = new XElement("Word",
                    new XElement("Id", newWord.Id),
                    new XElement("WordValue", newWord.WordValue),
                    new XElement("Category", enteredCategory),
                    new XElement("Definition", newWord.Definition),
                    new XElement("ImagePath", path)
                );

                XElement existingWord = doc.Root.Elements("Word")
                    .FirstOrDefault(w => w.Element("WordValue").Value == newWord.WordValue.ToString());

                XElement existingCategory = docCategs.Root.Elements("Category")
                    .FirstOrDefault(c => c.Element("CategoryName")?.Value == enteredCategory);

                if(newWord.Category == "" || newWord.Definition == "" || newWord.WordValue == "")
                {
                    MessageBox.Show("Please complete all mandatory (*) fields before adding a word!");
                }
                else
                {
                    if(existingWord != null)
                    { 
                        MessageBox.Show("ERROR - Word exists in dictionary!\nPlease add a different word\nor modify the" +
                            " existing one from the\n\"Modify Word\" menu option.");
                    }
                    else
                    {
                        if (existingCategory == null)
                        {
                            int maxId = docCategs.Root.Elements("Category").Select(c => (int)c.Element("Id")).DefaultIfEmpty(0).Max() + 1;

                            XElement newCategory = new XElement("Category",
                                new XElement("Id", maxId),
                                new XElement("CategoryName", enteredCategory));

                            docCategs.Root.Add(newCategory);
                            docCategs.Save("Resources/categories.xml");
                            categories = GetCategoriesFromXml("Resources/categories.xml");
                        }

                        doc.Root.Add(newWordElement);
                    }
                }
                doc.Save(filePath);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding word to XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ModifyWordInXml(string wordValue, string newCategory, string newDefinition, string newImagePath, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement existingWord = doc.Root.Elements("Word")
                    .FirstOrDefault(w => w.Element("WordValue").Value == wordValue);

                if (existingWord != null)
                {
                    existingWord.Element("Category").Value = newCategory;
                    existingWord.Element("Definition").Value = newDefinition;
                    existingWord.Element("ImagePath").Value = newImagePath;

                    doc.Save(filePath);
                }
                else
                {
                    MessageBox.Show("Error - Word not found in dictionary!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error modifying word in XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteWordFromXml(string word, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement wordToDelete = doc.Root.Elements("Word")
                    .FirstOrDefault(w => w.Element("WordValue")?.Value == word);

                if (wordToDelete != null)
                {
                    wordToDelete.Remove();
                    doc.Save(filePath);
                    LoadData();
                }
                else
                {
                    MessageBox.Show($"Word '{word}' not found in the dictionary.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting word from XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
