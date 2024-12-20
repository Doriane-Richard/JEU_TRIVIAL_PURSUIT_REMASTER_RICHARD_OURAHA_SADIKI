using System.Xml.Serialization;
using System;

namespace Trivial_Pursuit.Jeu.Entites;

[XmlRoot("Reponse")]

// Classe de réponse à une question
public class Reponse
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("texte")]
    public string Texte { get; set; }

    [XmlAttribute("correct")]
    public bool EstCorrecte { get; set; }
    /// <summary>
    /// Constructeur de la réponse
    /// </summary>
    /// <param name="texte">Texte de la réponse</param>
    /// <param name="correct">booléen disant si la réponse est juste ou fausse</param>
    public Reponse(string texte, bool correct)
    {
        Texte = texte;
        EstCorrecte = correct;
    }
    
    public Reponse() {}
}