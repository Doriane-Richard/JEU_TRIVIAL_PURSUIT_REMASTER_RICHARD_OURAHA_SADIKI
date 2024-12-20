using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    public class Sprite
    {
        [XmlAttribute("texture")]
        public string Type { get; set; }
    
        [XmlAttribute("positionX")]
        public int PositionX { get; set; }
    
        [XmlAttribute("positionY")]
        public int PositionY { get; set; }
    
        [XmlAttribute("size")]
        public int Size { get; set; }
        
        [XmlAttribute("sizeMin")]
        public int SizeMin { get; set; }
        [XmlAttribute("couleur")]
        public Enum.Couleur Couleur { get; set; }
        
        public Sprite(){}

        public Sprite(string type, int positionX, int positionY, int size,int  sizeMin, Enum.Couleur couleur)
        {
            Type = type;
            PositionX=positionX;
            PositionY=positionY;
            Size=size;
            SizeMin=sizeMin;
            Couleur=couleur;
        }
    }
}


