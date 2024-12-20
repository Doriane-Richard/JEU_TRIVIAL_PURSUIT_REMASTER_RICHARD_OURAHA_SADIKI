using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable] 
    public class Case
    {
        [XmlAttribute("type")]
        public Enum.TYPECASE Type { get; set; }
    
        [XmlAttribute("couleur")]
        public Enum.Couleur Couleur { get; set; }
    
        [XmlElement("Sprite")]
        public Sprite Sprite { get; set; }

        
        public Case(){}

        public Case(Enum.TYPECASE type, Enum.Couleur couleur, Sprite sprite)
        {
            Type = type;
            Couleur = couleur;
            Sprite = sprite;
            
        }
    }
}

