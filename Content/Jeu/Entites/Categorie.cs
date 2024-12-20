using System;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using TrivialPursuit.Content.Jeu.Entites;

namespace Trivial_Pursuit.Jeu.Entites;

// Classe représentant une catégorie de carte avec son nom et sa couleur et son identifiant
public class Categorie
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("nom")]
    public string Nom { get; set; }
    
    [XmlElement("Couleur")]
    public string CouleurHex { get; set; }

    [XmlIgnore]
    public Color Couleur
    {
        get
        {
            return CouleurConvert.DeHexaACouleur(CouleurHex);
        }
        set
        {
            CouleurHex = CouleurConvert.DeCouleurAHexa(value);
        }
    }
    
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    public Categorie()
    {
        Nom = "Sans categorie";
        Couleur = Color.White;
        Id = 0;
    }

    /// <summary>
    /// Initialise la catégorie avec les paramètres données
    /// </summary>
    /// <param name="nom">Le nom de la catégorie</param>
    /// <param name="couleur">>La couleur en rgb</param>
    public Categorie(string nom, Color couleur)
    {
        Nom = nom;
        Couleur = couleur;
    }    
}