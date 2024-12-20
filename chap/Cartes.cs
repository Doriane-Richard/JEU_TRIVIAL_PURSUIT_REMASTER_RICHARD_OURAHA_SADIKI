using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    public class Cartes
    {
        [XmlElement("Carte")]
        public List<Carte> CarteList { get; set; } = new List<Carte>();
        public Cartes(){}

        public Cartes(List<Carte> cartes)
        {
            CarteList = cartes;
        }
        
        public override string ToString()
        {
            if (CarteList ==null )
            {
                return "La liste des cartes est vide.";
            }
            var res = "\tCartes:\n";
            foreach (Carte ct in CarteList)
            {
                res += $"\n{ct}\n";
            }
            return res;
        }
    }
}

