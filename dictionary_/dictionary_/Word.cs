using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dictionary_
{
    [Serializable]
    [XmlRoot("Words")]
    internal class Word
    {
        [XmlElement]
        public int Id { get; set; }

        [XmlElement]
        public string WordValue { get; set; }

        [XmlElement]
        public string Category { get; set; }

        [XmlElement]
        public string Definition { get; set; }

        [XmlElement]
        public string ImagePath { get; set; }

        public Word() { }
    }
}
