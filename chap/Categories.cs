
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrivialPursuit.chap
{
    [Serializable]
    [XmlRoot("Categories",Namespace ="http://www.univ-grenoble-alpes.fr/l3miage/trivialpursuit")]
    public class Categories
    {
    [XmlElement("Categorie")]
    public List<Categorie> CategoriesList { get; set; }

    public Categories()
    {
        
    }

    public Categories(List<Categorie> categoriesList)
    {
        CategoriesList = categoriesList;
    }
    
    public override string ToString()
    {
        if (CategoriesList ==null )
        {
            return "La liste des categories est vide.";
        }
        var res = "Catégories:\n";
        foreach (Categorie cat in CategoriesList)
        {
            res += $"\n{cat}\n";
        }
        return res;
    }
    }
}

