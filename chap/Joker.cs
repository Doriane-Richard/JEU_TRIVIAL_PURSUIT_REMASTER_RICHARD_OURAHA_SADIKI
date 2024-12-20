using System;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    public class Joker
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        
        [XmlAttribute("nom")]
        public string Nom { get; set; }
        
        [XmlAttribute("effet")]
        public string Effet { get; set; }

        public Joker()
        {
            
        }

        public Joker(int id, string nom, string effet)
        {
            Id = id;
            Nom = nom;
            Effet = effet;
        }
    }
}

