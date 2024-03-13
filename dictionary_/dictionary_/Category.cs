using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dictionary_
{
    [Serializable]
    [XmlRoot("Categories")]
    internal class Category
    {
        [XmlElement]
        public int Id {  get; set; }

        [XmlElement]
        public string CategoryName { get; set; }

        public Category() { }
    }
}
