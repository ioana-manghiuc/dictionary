using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
                        GivenName = a.Element("GivenName").Value,
                        FamilyName = a.Element("FamilyName").Value,
                        DateOfBirth = a.Element("DateOfBirth").Value,
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

        public void AddAdminToXml(Admin admin, string filePath)
        {
            try
            {
                XDocument document = XDocument.Load(filePath);

                XElement newAdmin = new XElement("Admin",
                    new XElement("Id", admin.Id),
                    new XElement("GivenName", admin.GivenName),
                    new XElement("FamilyName", admin.FamilyName),
                    new XElement("DateOfBirth", admin.DateOfBirth),
                    new XElement("Email", admin.Email),
                    new XElement("Password", admin.Password)
                    );

                XElement existingAdmin = document.Root.Elements("Admin")
                    .FirstOrDefault(a => a.Element("Email").Value == admin.Email.ToString());

                if (admin.FamilyName == "" || admin.GivenName == "" || admin.Email == "" || admin.Password == "" || admin.DateOfBirth == "")
                {
                    MessageBox.Show("Please complete all fields to register!");
                }
                else
                {
                    if(existingAdmin == null)
                    {
                        document.Root.Add(newAdmin);
                        document.Save(filePath);
                        LoadData();
                    }
                    else
                    {  
                        MessageBox.Show("ERROR - admin exists!\nPlease log in.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding admin to XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
