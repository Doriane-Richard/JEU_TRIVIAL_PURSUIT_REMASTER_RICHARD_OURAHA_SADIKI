
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace TrivialPursuit.chap
{
    [Serializable]
    public class Joueur
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        
        [XmlAttribute("nom")]
        public string Nom { get; set; }
        
        [XmlAttribute("score")]
        public int Score { get; set; }
        
        [XmlElement("Pion")]
        public Pion Pion { get; set; }
        
     [XmlElement("Jokers")]
        public Jokers List { get; set; } 

        public Joueur()
        {
            
        }

        public Joueur(int id, string nom, int score, Pion pion, Jokers jokers)
        {
            Id = id;
            Nom = nom;
            Score = score;
            Pion = pion;
            List = jokers;
        }
    }
}

