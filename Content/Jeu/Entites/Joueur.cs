using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Trivial_Pursuit.Jeu.Enumeration;

namespace Trivial_Pursuit.Jeu.Entites;

/// Classe représentant un joueur dans une partie, il a un nom, est affecté a une case, a un score, une liste de joker, un joker actif et un état
public class Joueur : Sprite
{
    private string _nom;
    private Case _case;
    public bool _joker5050Utilise { get; set; }
    public int Score { get; private set; }
    private List <Joker> _jokers;
    public Joker JokerActif { get; set; }
    public EtatJoueur Etat { get; set; }

    /// <summary>
    /// Constructeur de Joueur qui l'initialise avec un nom, une texture, une position et une case de départ et son état de base qui est normal
    /// </summary>
    /// <param name="nom">Le nom du juoeur</param>
    /// <param name="texture">La texture du joueur</param>
    /// <param name="position">Sa position initiale</param>
    /// <param name="caseD">La case de départ du joueur (la case de départ du plateau)</param>
    public Joueur(string nom,Texture2D texture, Vector2 position, Case caseD) : base(texture, position,60)
    {
        _nom = nom; 
        _case = caseD; // case de départ
        Score = 0;
        _jokers = new List<Joker>();
        Etat = EtatJoueur.Normal;
        JokerActif = new Joker("","");
        _joker5050Utilise = false;
    }
    
    public string GetNom()
    {
        return _nom;
    }

    public string GetScore()
    {
        return Score.ToString();
    }
    
    public Case GetCase()
    {
        return _case;
    }
    
    /// <summary>
    /// Modifie la case du joueur et le place à la position de sa nouvelle case
    /// </summary>
    /// <param name="nouvelleCase">La nouvelle case du joueur</param>
    public void SetCase(Case nouvelleCase)
    {
        _case = nouvelleCase;
        _position = nouvelleCase.GetPosition();
        SetPositionSurCase();
    }

    /// <summary>
    /// Place le joueur sur sa case
    /// </summary>
    public void SetPositionSurCase()
    {
        _position = _case.GetPosition();
        SetPosition(new Vector2(_position.X+40,_position.Y+40));
    }
    
    public void SetPosition(Vector2 position)
    {
        _position = position;
    }
    
    public Vector2 GetPosition()
    {
        return _position;
    }
    
    /// <summary>
    /// Active le mode ChoixDifficulte du joueur et le place au centre du jeu
    /// </summary>
    public void ActiverChoixDifficulte()
    {
        Etat = EtatJoueur.ChoixDifficulte;
        SetPosition(new Vector2(900, 400));
    }

    public List<Joker> GetJokers()
    {
        return _jokers;
    }
    
    // Ajoute un joker à un joueur qu'il ne posséde pas encore
    /// <summary>
    /// Ajoute un des deux joker au joueur qu'il n'a pas de manière aléatoire
    /// S'il a déjà deux jokers, lui en rajoute un aléatoire
    /// </summary>
    public void AjouterRandomJoker()
    {

        // initialise la liste des joker
        var typesDeJokers = new List<Joker>
        {
            new Joker("50/50", "Supprime deux mauvaises réponses lors d’une question à choix multiple."),
            new Joker("Relance de question", "Permet de changer la question actuelle par une autre du même thème.")
        };
        
        // recupere un joker aléatoire
        Random rnd = new Random();
        Joker jokerAleatoire = typesDeJokers[rnd.Next(typesDeJokers.Count)];
        _jokers.Add(jokerAleatoire);
    }
    
    /// <summary>
    /// Fais jouer le joker au joueur
    /// Si le joker est 50/50, change l'attribut jokerActif du joueur
    /// Si le joker est Relance, appel la méthode correspondant dans la classe Joker
    /// </summary>
    /// <param name="partie">La partie en cours</param>
    /// <param name="joker">Le joker à jouer</param>
    public void JouerJoker(Partie partie, Joker joker)
    {
        // Supprime le premier joker correspondant dans la liste des joker
        int i = 0;
        while (i < _jokers.Count && _jokers[i] != joker)
        {
            i++;
        }
        
        if (_jokers[i].getNom().Equals("50/50"))
        {
            JokerActif = _jokers[i];
            _joker5050Utilise = true;
        }
        else if (_jokers[i].getNom().Equals("Relance de question"))
        {
            _jokers[i].JouerRelance(partie);
            JokerActif = _jokers[i];
            _joker5050Utilise = false;
        }
        _jokers.Remove(_jokers[i]);
    }

    /// <summary>
    /// Fais jouer la réponse au joueur. Si la reponse est correcte, augmente la score du joueur puis change son état
    /// Si le joueur répond juste sur une case chance, le fait rejouer
    /// </summary>
    /// <param name="reponse">La réponse choisi pour le joueur à la question</param>
    /// <returns></returns>
    public bool JouerReponse(Reponse reponse)
    {
      
        Etat = EtatJoueur.Normal;
        if (reponse.EstCorrecte)
        {
            if (_case.Type == TypeCase.CHANCE)
            {
                Etat = EtatJoueur.Rejouer;
            }
            Score++;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Met à jour la position du joueur lors de l'appuie sur les fléches du clavier s'il doit répondre à une question ou choisir une difficulté
    /// </summary>
    public void Update()
    {
        if (Etat==EtatJoueur.ChoixDifficulte || Etat==EtatJoueur.ChoixReponse)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up)&& _position.Y>50)
            {
                _position.Y -= 8; 
            }

            if (keyboardState.IsKeyDown(Keys.Down) && _position.Y<850)
            {
                _position.Y += 8;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && _position.X<1550)
            {
                _position.X += 8;
            }

            if (keyboardState.IsKeyDown(Keys.Left) && _position.X>50)
            {
                _position.X -= 8;
            }
        }
    }
    
    /// <summary>
    /// Dessine le joueur
    /// </summary>
    /// <param name="spriteBatch">Utilisé pour dessiner le joueur</param>
    public new void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}