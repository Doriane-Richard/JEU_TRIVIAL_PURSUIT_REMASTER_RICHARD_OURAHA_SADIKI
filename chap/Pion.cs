using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    public class Pion
    {
        [XmlAttribute("name")]
        public string Nom { get; set; }
        
        [XmlAttribute("imageLink")]
        public string ImageLink { get; set; }
        
        [XmlAttribute("description")]
        public string Description { get; set; }
        
        [XmlAttribute("case")]
        public int Case { get; set; }
        
        public Pion(){}

        public Pion(string nom, string imageLink, string description, int c)
        {
            Nom = nom;
            ImageLink = imageLink;
            Description = description;
            Case = c;
        }
    }
}

