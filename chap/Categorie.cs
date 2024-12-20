using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    public class Categorie
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        
        [XmlAttribute("nom")]
        public string Nom { get; set; }
        
        [XmlAttribute("couleur")]
        public  Enum.Couleur Couleur { get; set; }
        
        [XmlElement("Cartes")]
        public Cartes C { get; set; }

        public Categorie()
        {
            
        }

        public Categorie(int id, string nom, Enum.Couleur couleur,Cartes cartes)
        {
            Id = id;
            Nom = nom;
            Couleur = couleur;
            C= cartes;
        }

        public override string ToString()
        {
            
            return $"\nId: {Id}, Nom: {Nom}, Couleur: {Couleur}\n{C}";
        }
    }
}

