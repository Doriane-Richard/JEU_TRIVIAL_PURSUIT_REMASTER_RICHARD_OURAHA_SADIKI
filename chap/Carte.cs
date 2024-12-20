
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    public class Carte
    {
        [XmlAttribute("id")]    
        public int Id { get; set; }

        [XmlAttribute("difficulte")]
            public Enum.Difficulte Difficulte { get; set; }
        
        [XmlElement("Question")]
        public string Question { get; set; }
        
        [XmlElement("Reponses")]
        public Reponses Reponses { get; set; }
        public Carte(){}

        public Carte(int id, Enum.Difficulte difficulte, string question, Reponses reponses)
        {
            Id = id;
            Difficulte = difficulte;
            Question = question;
            Reponses = reponses;
        }

        public override string ToString()
        {
            
            return $"\tId: {Id}, Difficulte: {Difficulte}, Question: {Question},\n {Reponses}";
        }
    
    }
}