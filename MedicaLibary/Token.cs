using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MedicaLibary
{
    [XmlRoot(ElementName = "patient")]
    public class Patient
    {
        [XmlElement(ElementName = "imie")]
        public string Imie { get; set; }
        [XmlElement(ElementName = "nazwisko")]
        public string Nazwisko { get; set; }
        [XmlElement(ElementName = "pesel")]
        public string Pesel { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "lib")]
    public class Lib
    {
        [XmlElement(ElementName = "patient")]
        public List<Patient> Patient { get; set; }
    }

}
