using System;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    public class Background
    {
        [XmlAttribute("texture")]
        public string Texture { get; set; }
        public Background(){}
        public Background(string texture)
        {
            Texture = texture;
        }
    }
    
    
}

