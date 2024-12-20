using System;
using System.Xml.Serialization;
namespace TrivialPursuit.chap
{
    public class Enum
    {
        public enum TYPECASE
        {
            [XmlEnum("VIDE")] VIDE,
            [XmlEnum("QUESTION")] QUESTION,
            [XmlEnum("CHANCE")] CHANCE,
            [XmlEnum("CHALLENGE")] CHALLENGE,
            [XmlEnum("JOKER")] JOKER
        }
        
        public enum Couleur
        {
            [XmlEnum("BLEU")] BLEU,
            [XmlEnum("ROUGE")] ROUGE,
            [XmlEnum("JAUNE")] JAUNE,
            [XmlEnum("VERT")] VERT,
            [XmlEnum("ORANGE")] ORANGE,
            [XmlEnum("ROSE")] ROSE,
            [XmlEnum("GRIS")] GRIS
            
        }
        
        public enum Difficulte
        {
            [XmlEnum("Facile")] Facile,
            [XmlEnum("Moyen")] Moyen,
            [XmlEnum("Difficile")] Difficile,
            
        }
    }
}

