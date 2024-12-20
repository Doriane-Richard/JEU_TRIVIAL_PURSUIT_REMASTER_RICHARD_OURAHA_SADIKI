
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    [XmlRoot("Partie")]
    public class partie
    {
        [XmlElement("Cases")] public Cases Cases { get; set; }

        [XmlElement("Cartes")] public Cartes Cartes { get; set; }

        public partie()
        {
        }

        public partie(Cases cases, Cartes cartes)
        {
            Cases = cases;
            Cartes = cartes;
        }
    }
}
