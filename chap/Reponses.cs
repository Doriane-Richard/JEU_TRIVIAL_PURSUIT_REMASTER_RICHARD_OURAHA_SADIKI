using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    public class Reponses
    {
        [XmlElement("Reponse")] 
        public List<Reponse> ResponsesList { get; set; } 

        public Reponses()
        {
            
        }

        public Reponses(List<Reponse> responsesList)
        {
            ResponsesList = responsesList;
        }

        public override string ToString()
        {
            if (ResponsesList ==null )
            {
                return "La liste des reponses est vide.";
            }
            var res = "\tRéponses:\n";
            foreach (Reponse reponse in ResponsesList)
            {
                res += $"\n{reponse}\n";
            }
            return res;
        }
    }
}