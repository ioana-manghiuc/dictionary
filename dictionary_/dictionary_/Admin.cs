using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dictionary_
{
    [Serializable]
    [XmlRoot("Administrators")]
    internal class Admin
    {
        [XmlElement]
        public int Id { get; set; }

        [XmlElement]
        public string GivenName { get; set; }

        [XmlElement]
        public string FamilyName { get; set; }

        [XmlElement]
        public string DateOfBirth { get; set; }
        public string Email { get; set; }

        [XmlElement]
        public string Password { get; set; }

        public Admin() { }

    }
}
