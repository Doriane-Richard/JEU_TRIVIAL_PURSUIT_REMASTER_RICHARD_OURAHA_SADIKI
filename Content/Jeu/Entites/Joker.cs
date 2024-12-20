using System;
using System.Collections.Generic;
using System.Linq;
using Trivial_Pursuit.Jeu.Enumeration;

namespace Trivial_Pursuit.Jeu.Entites;

/// <summary>
/// Représente un Joker qui est un bonus utilisable une unique fois par un joueur, il contient un nom et la description de son effet
/// Bien que l'attribut effet ne soit jamais utilisé, il est tout de même présent pour une future mise à jour par exemple
/// </summary>
public class Joker
{
    private string _nom;
    private string _effet;


    /// <summary>
    /// Initialise le joker
    /// </summary>
    /// <param name="nom">Le nom du joker</param>
    /// <param name="effet">La description du joker</param>
    public Joker(string nom, string effet)
    {
        _nom = nom;
        _effet = effet;
    }

    public string getNom()
    {
        return _nom;
    }
    
    /// <summary>
    /// Donne une réponse juste et une réponse fausse pour une question
    /// </summary>
    /// <param name="carte">La carte contenant la question les réponses</param>
    /// <returns>Une question juste et une question fausse dans la carte</returns>
    public List<Reponse> Jouer5050(Carte carte)
    {
        List<Reponse> reponses5050 = new List<Reponse>();
        
        List<Reponse> reponses = carte.Reponses;
        
        int numeroReponseFausse;
        int positionReponseJuste = 0;
        
        Reponse reponseFausse;
        Random random = new Random();
        
        for (int i = 0; i < reponses.Count - 1; i++)
        {
            if (reponses[i].EstCorrecte)
            {
                reponses5050.Add(reponses[i]);
                positionReponseJuste = i;
                break;
            }
            
        }
        
        numeroReponseFausse = random.Next(0, reponses.Count);

        while (reponses[numeroReponseFausse] == reponses[positionReponseJuste])
        {
            numeroReponseFausse = random.Next(0, reponses.Count);
        }
        
        reponseFausse = reponses[numeroReponseFausse];
        
        reponses5050.Add(reponseFausse);
        
        return reponses5050;
    }
    
    /// <summary>
    /// Joue le joker de relance de question pour la partie changeant la carte piochée de la partie en cours
    /// </summary>
    /// <param name="partie">Partie en cours</param>
    public void JouerRelance(Partie partie)
    {
        partie.PiocherCarte(partie.CartePiochee.Categorie, partie.CartePiochee.Difficulte);
        Console.WriteLine("Nouvelle carte jouee: " + partie.CartePiochee.Question);
    }
    
}