using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace TrivialPursuit.chap
{
    [Serializable]
    public class Cases{
    
         [XmlElement("Case")]
         public List<Case> CasesList { get; set; }

         public Cases(){}

         public Cases(List<Case> cases)
         {
             CasesList=cases;
         }

    }
}


