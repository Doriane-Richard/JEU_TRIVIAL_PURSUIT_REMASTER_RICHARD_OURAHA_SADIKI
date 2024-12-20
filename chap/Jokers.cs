
using System;
using System.Collections.Generic;
using System.Xml.Serialization;



namespace TrivialPursuit.chap
{
    [Serializable] 
    public class Jokers
    {
      [XmlElement("Joker")] 
      public List<Joker> J { get; set; }

        public Jokers()
        {

        }

        public Jokers(List<Joker> jokers)
        {
            J = jokers;
        }

    }
}