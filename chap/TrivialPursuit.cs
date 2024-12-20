using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace TrivialPursuit.chap

{
    [Serializable]
    //[XmlRoot("TrivialPursuit",Namespace ="http://www.univ-grenoble-alpes.fr/l3miage/trivialpursuit")]
    public class TrivialPursuit
    {
        [XmlElement("Plateau")]
        public Plateau Plateau { get; set; }
        // Emplacement du schéma XML (attribut XSI).
        [XmlAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlElement("Categories")]
        public Categories Categories { get; set; }
        
         [XmlElement("Joueurs")]
        public Joueurs Js { get; set; } 
        public TrivialPursuit()
        {
            
        }
        
        public TrivialPursuit(Plateau plateau,Categories categories,Joueurs joueurs,string schemaLocation)
        {
            Plateau = plateau;
            Categories = categories;
            Js = joueurs;
            SchemaLocation = schemaLocation;
        }
    }
}