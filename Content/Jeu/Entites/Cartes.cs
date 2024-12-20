using System.Collections.Generic;
using System.Xml.Serialization;
using Trivial_Pursuit.Jeu.Entites;

namespace TrivialPursuit.Content.Jeu.Entites;

[XmlRoot("Cartes", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/trivialpursuit")]
// Classe utilisé uniquement pour la sérialization, contient un ensemble de cartes
public class Cartes
{
    [XmlElement("Carte")]
    public List<Carte> ListeCartes { get; set; }

    public Cartes()
    {
        ListeCartes = new List<Carte>();
    }
}