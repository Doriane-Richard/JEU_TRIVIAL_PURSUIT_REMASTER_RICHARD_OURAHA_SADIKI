using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    public class Plateau
    {
        [XmlElement("Cases")] 
        public Cases Cases { get; set; } 
        [XmlElement("Background")]
        public Background Background { get; set; }
        
        public Plateau(){}

        public Plateau(Background background, Cases cases)
        {
            Background = background;
            Cases = cases;
        }
    }
}

