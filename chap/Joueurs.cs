using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    public class Joueurs
    {
        [XmlElement("Joueur")]
        public List<Joueur> JoueursList { get; set; }

        public Joueurs()
        {
           
        }

        public Joueurs(List<Joueur> joueursList)
        {
            JoueursList = joueursList;
        }
    }
}