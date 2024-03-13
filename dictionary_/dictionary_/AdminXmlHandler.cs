using System;
using System.Collections.Generic;
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
    internal class AdminXmlHandler
    {
        [XmlArray]
        List<Admin> admins {  get; set; }

        public AdminXmlHandler() { LoadData(); }

        private void LoadData()
        {
            admins = GetAdminsFromXml("Resources/admins.xml");
        }
        private List<Admin> GetAdminsFromXml(string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);
                return doc.Root.Elements("Admin")
                    .Select(a => new Admin
                    {                        
                        Id = int.Parse(a.Element("Id").Value),
                        Email = a.Element("Email").Value,
                        Password = a.Element("Password").Value

                    }).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading admins from XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Admin>();
            }
        }

        public List<Admin> GetAdmins() { return admins; }

        public bool FindAdmin(Admin admin)
        {
            return admins.Any(a => a.Email == admin.Email && a.Password == admin.Password);
        }
    }
}
