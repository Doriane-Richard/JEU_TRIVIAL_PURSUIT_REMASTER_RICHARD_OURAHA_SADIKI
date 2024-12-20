using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Trivial_Pursuit.Jeu.Enumeration;

namespace Trivial_Pursuit.Jeu.Entites;

/// Classe représentant une unique partie ayant un début et une fin
/// Elle contient un plateau, la liste des joueurs qui la composent, un ensemble de cartes, de cartes jouées (le tas) ainsi que le tour du joueur a jouer
public class Partie
{
    private Plateau _plateau;
    public List<Joueur> Joueurs { get; set; }
    private List<Carte> _cartes;
    private List<Carte> _cartesJouees;
    public Carte CartePiochee { get; set; }
    public int TourJoueur { get; set; }

    /// <summary>
    /// initialise la partie avec les arguments données
    /// commence la partie par le joueur 0 qui est le premier joueur (le joueur numéro 1 en d'autres thermes)
    /// </summary>
    /// <param name="plateau">un plateau contenant un ensemble de cases</param>
    /// <param name="joueurs">la liste des joueurs qui composeront la partie</param>
    /// <param name="cartes">Les cartes de la partie auxquels les joueurs pouront répondre</param>
    public Partie(Plateau plateau, List<Joueur> joueurs,List<Carte> cartes)
    {
        _plateau = plateau;
        Joueurs = joueurs;
        _cartes = cartes;
        _cartesJouees = new List<Carte>();
        CartePiochee = null;
        TourJoueur = 0;
    }

    /// <summary>
    /// Fais jouer un tour au joueur donc ça l'est.
    /// - Lance le dé et place modifie la case du joueur en conséquence ainsi que sa position
    /// - Prends en compte les différents cas de figure des cases (entre joker, chance, question et vide
    /// </summary>
    public void JouerTour()
    {
        // LANCER DE DÉ ET UPDATE DE LA POSITION DU JOUEUR
        Joueur joueurActif = Joueurs[TourJoueur];
        Case caseJoueur = joueurActif.GetCase();
        int valeurDe = LancerDe();
        
        int numeroCaseActuelle = _plateau.GetIndiceCase(caseJoueur);
        int numeroNouvelleCase = (numeroCaseActuelle + valeurDe) % _plateau.GetNombreCases();
        Case nouvelleCase = _plateau.GetCase(numeroNouvelleCase);

        Console.WriteLine($"{joueurActif.GetNom()} avance de {valeurDe} case(s) et se retrouve sur la case {numeroNouvelleCase}.");

        // Set la case du nouveau joueur et modifie la position du joueur
        joueurActif.SetCase(nouvelleCase);
        DecalageSiSuperposition(joueurActif);

        switch (joueurActif.GetCase().Type)
        {
            case TypeCase.JOKER:
                joueurActif.AjouterRandomJoker();
                // Choisi le prochain joueur à jouer
                TourJoueur = (TourJoueur + 1) % Joueurs.Count;
                break;
            
            case TypeCase.CHANCE:
                joueurActif.Etat = EtatJoueur.AttenteConfirmation;
                break;
            
            case  TypeCase.QUESTION:
                joueurActif.Etat = EtatJoueur.AttenteConfirmation;
                break;
            
            case TypeCase.VIDE:
                // Choisi le prochain joueur à jouer
                TourJoueur = (TourJoueur + 1) % Joueurs.Count;
                break;
        }
    }
    
    /// <summary>
    /// Pioche une carte aléatoire dans la liste des cartes de la partie en fonction de la catégorie et de la difficulté en excluant les cartes déjà jouéees.
    /// Si toutes les cartes de la catégorie et difficultés sont jouées, pioche une carte aléatoire dans la liste de celles déjà jouées.
    /// </summary>
    /// <param name="categorie">Catégorie de la carte à piocher</param>
    /// <param name="difficulte">Difficulté de la carte à piocher</param>
    public void PiocherCarte(Categorie categorie, Difficulte difficulte)
    {
        // Recupere les cartes de la bonne difficulté et bonne catégorie
        var cartesFiltrees = _cartes.Where(c => c.Categorie == categorie && c.Difficulte == difficulte && !_cartesJouees.Contains(c)).ToList();

        Console.WriteLine("Difficulte: " + difficulte + " Categorie: " + categorie);
        Console.WriteLine("Nombre de carte filtrees: " + cartesFiltrees.Count);
        
        // Si toute les cartes de la catégorie et difficulté déjà jouées, on autorise à rejouer les cartes
        if (cartesFiltrees.Count == 0)
        {
            Console.WriteLine("Toute les cartes de la catégorie {0} et de difficulté {1} déjà jouées, on rejoue des cartes",categorie,difficulte);
            cartesFiltrees = _cartes.Where(c => c.Categorie == categorie && c.Difficulte == difficulte).ToList();
        }
        
        // Choisit une carte aléatoire dans le lot
        Random rnd = new Random();
        
        CartePiochee = cartesFiltrees[rnd.Next(cartesFiltrees.Count)];
        _cartesJouees.Add(CartePiochee);
    }
    
    /// <summary>
    /// Décale la position des joueurs pour éviter la superposition dans les cases
    /// </summary>
    /// <param name="joueur">Joueur à décaler</param>
    private void DecalageSiSuperposition(Joueur joueur)
    {
        // check si un autre joueur est sur la même case
        foreach (var autreJoueur in Joueurs)
        {
            if (joueur != autreJoueur && joueur.GetCase() == autreJoueur.GetCase())
            {
                joueur.SetPosition(new Vector2( joueur.GetPosition().X + 30,  joueur.GetPosition().Y));
            }
        }
    }

    /// <summary>
    /// Tire un nombre aléatoire entre 1 et 6, utilisé pour savoir de combien de cases un joueur avancera après son tour
    /// </summary>
    /// <returns>le nombre tiré</returns>
    public int LancerDe()
    {
        Random random = new Random();
        return random.Next(1, 7);
    }
    
    /// <summary>
    /// Vérifie un des joueurs a remporté la partie, c'est à dire a un score de 6
    /// </summary>
    /// <returns>true si un joueur a gagné, false sinon</returns>
    public bool EstTerminee()
    {
        foreach (var joueur in Joueurs)
        {
            if (joueur.Score == 6)
            {
                return true;
            }
        }
        return false;
    }
}