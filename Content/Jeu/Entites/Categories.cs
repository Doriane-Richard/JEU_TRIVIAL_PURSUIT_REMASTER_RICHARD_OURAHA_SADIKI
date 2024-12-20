using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Trivial_Pursuit.Jeu.Entites;

namespace TrivialPursuit.Content.Jeu.Entites;


[XmlRoot("Categories")]

// Classe contenant l'ensemble des différentes catégories du jeu
public class Categories
{
    [XmlElement("Categorie")]
    public List<Categorie> ListeCategories { get; set; }
    
    /// <summary>
    /// Initialise la liste des catégories
    /// </summary>
    public Categories()
    {
        ListeCategories = new List<Categorie>();
    }
    
    public Categorie GetCategorieViaNom(string name)
    {
        return ListeCategories.FirstOrDefault(c => c.Nom.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    
    public Categorie GetCategorieViaId(int id)
    {
        return ListeCategories.FirstOrDefault(c => c.Id == id);
    }
    
}