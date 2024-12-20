using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Trivial_Pursuit.Jeu.Enumeration;

namespace Trivial_Pursuit.Jeu.Entites;

using System.Collections.Generic;
using Trivial_Pursuit.Jeu.Enumeration;

[XmlRoot("Carte")]


// Représente une carte de question dans le jeu, elle contient une question avec des réponses (habituellement 4), elle a une catégorie et une difficulté
public class Carte
{
    
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlAttribute("categorieId")]
    public int CategorieId { get; set; } 
    
    [XmlArray("Reponses")]
    [XmlArrayItem("Reponse")]
    public List<Reponse> Reponses { get; set; }    
    public Categorie Categorie { get; set; }

    [XmlElement("Question")]
    public string Question { get; set; }
    
    [XmlIgnore]
    public Difficulte Difficulte{get;set;}
    
    [XmlAttribute("difficulte")]
    public string DifficulteValeur
    {
        get => Difficulte.ToString();
        set
        {
            if (Enum.TryParse(value, true, out Difficulte difficulte))
            {
                Difficulte = difficulte;
            }
            else
            {
                throw new InvalidOperationException("Valeur de difficulté invalide :" + value);
            }
        }
    }

    public Carte()
    {
        Reponses = new List<Reponse>();
    }
    
    /// <summary>
    /// Constructeur de Carte
    /// </summary>
    /// <param name="categorie">La catégorie de la carte</param>
    /// <param name="question">La question</param>
    /// <param name="reponses">Les réponses à la question</param>
    /// <param name="difficulte">La difficulté de la carte</param>
    public Carte(Categorie categorie, string question, List<Reponse> reponses, Difficulte difficulte)
    {
        Categorie = categorie;
        Question = question;
        Reponses = reponses;
        Difficulte = difficulte;
    }
    

    /// <summary>
    /// Compare le texte d'une réponse donnée avec la réponse correcte à la question pour savoir si la réponse donnée est juste
    /// </summary>
    /// <param name="reponse">réponse donnée</param>
    /// <returns></returns>
    public bool EstCorrecte(Reponse reponse)
    {
        foreach (var r in Reponses)
        {
            if (r.Texte == reponse.Texte && r.EstCorrecte)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Mélange aléatoirement les réponses d'une carte (algo de Fisher-Yates)
    /// </summary>
    public void MelangerReponses()
    {
        Random rng = new Random();
        for (int i = Reponses.Count - 1; i >= 0; i--)
        {
            int j = rng.Next(0, i + 1);
            var temp = Reponses[i];
            Reponses[i] = Reponses[j];
            Reponses[j] = temp;
        }
    }
}