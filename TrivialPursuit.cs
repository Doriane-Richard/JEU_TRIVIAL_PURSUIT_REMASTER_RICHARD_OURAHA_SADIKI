using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Trivial_Pursuit.Jeu.Entites;
using Trivial_Pursuit.Jeu.Enumeration;
using TrivialPursuit.Content.Jeu.Entites;

namespace TrivialPursuit;

/// <summary>
/// Classe principale du jeu appellée au lancement du programme, elle se charge de la mise à jour du rendu ainsi que la manipulation des données
/// </summary>
public class TrivialPursuit : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private KeyboardState _ancienEtatClavier;
    private MouseState _ancienEtatSouris;
    private List<ElementInteractif> _elementReponsesChoisies;
    public List<Reponse> _Reponses5050;
    
    // Rendu
    private EtatJeu etatJeuActif { get; set; }
    
    // Texture
    private Texture2D _textureCase;
    private List<Texture2D> _textureJoueurs;
    private Texture2D _textureNeutre;
    
    // Fond
    private Texture2D _fondGeneral;
    private Texture2D _fondMenu;
    
    // Partie
    private Partie _partie;
    private Plateau _plateau;
    private List<Joueur> _joueurs;
    private String _phrasePrincipale;

    // Menu
    private MenuPage _menuPage;
    
    // Tableau des scores 
    private TableauDesScores _tableauDesScores;
    
    // Font
    private SpriteFont _fontCase;
    private SpriteFont _fontTitre;
    private SpriteFont _fontTexte;
    private SpriteFont _fontInstruction;
    private SpriteFont _fontReponse;
        
    // Couleur
    private Color _textCouleur = CouleurConvert.DeHexaACouleur("#d7c054");
    private Color _couleurReponseValide = CouleurConvert.DeHexaACouleur("#67f543");
    private Color _couleurReponseNonValide = CouleurConvert.DeHexaACouleur("#ff4c4c");
    
    public TrivialPursuit()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _textureJoueurs = new List<Texture2D>();
        _joueurs = new List<Joueur>();
        etatJeuActif = EtatJeu.MENU;
        _phrasePrincipale = "";
    }

    /// <summary>
    /// Initialise les paramètres de la fenêtre de jeu à savoir la taille et l'état initial du clavier
    /// Initialise une partie et un plateau en utilisant leur constructeurs
    /// </summary>
    protected override void Initialize()
    {
        // Réglage de la taille du plateau
        _graphics.PreferredBackBufferWidth = 1600;
        _graphics.PreferredBackBufferHeight = 900;
        _graphics.ApplyChanges();
        
        _ancienEtatClavier = Keyboard.GetState();
        _partie = new Partie(null, new List<Joueur>(), new List<Carte>());
        _plateau = new Plateau(null, new List<Case>());
        
        base.Initialize();
    }

    /// <summary>
    /// Load les images textures et polices nécessaires au jeu
    /// Load le contenue de la partie.
    /// Utilise la sérialization pour load les cartes et catégories
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Load les images
        _textureCase = Content.Load<Texture2D>("Images/case_3");
        _fondGeneral = Content.Load<Texture2D>("Images/fond_plateau");
        _fondMenu = Content.Load<Texture2D>("Images/fond_menu");
        _textureNeutre = Content.Load<Texture2D>("Images/fond_neutre");

        // Load l'image des joueurs 
        _textureJoueurs.Add(Content.Load<Texture2D>("Images/pion-rouge"));
        _textureJoueurs.Add(Content.Load<Texture2D>("Images/pion-bleu"));
        _textureJoueurs.Add(Content.Load<Texture2D>("Images/pion-jaune"));
        _textureJoueurs.Add(Content.Load<Texture2D>("Images/pion-vert"));
        
        // Load le font des cases
        _fontCase = Content.Load<SpriteFont>("font/FontCase");
        // Met ce font par défaut à toute les cases
        Case.SetFontCase(_fontCase);
        
        // Load les autres polices d'écritures
        _fontTitre = Content.Load<SpriteFont>("font/FontTitre");
        _fontTexte = Content.Load<SpriteFont>("font/FontTextePrincipal");
        _fontInstruction = Content.Load<SpriteFont>("font/FontInstruction");
        _fontReponse = Content.Load<SpriteFont>("font/fontReponse");
        
        // Load la page menu
        _menuPage = new MenuPage(_fondMenu, _fontTitre, _fontTexte, _textureCase, _fontInstruction);
        _menuPage.LancerPartie += LancerPartie;
        
        // Load tableau des scores
        _tableauDesScores = new TableauDesScores(_fondGeneral, _fontTitre, _fontTexte, _joueurs, _textureCase, _textCouleur);
        _tableauDesScores.RetourMenuClicked += TableauDesScores_RetourMenuClicked;
        
        // Serialize les différentes catégories et les stockes dans categories
        string basePath = AppContext.BaseDirectory;
        Categories categories = XMLUtils<Categories>.Deserialization(Path.Combine(basePath, @"..\..\..\xml\categories.xml"));

        Categorie sport = categories.GetCategorieViaNom("Sport");
        Categorie zytho = categories.GetCategorieViaNom("Zythologie");
        Categorie jeuxVideo = categories.GetCategorieViaNom("Jeux vidéo");
        Categorie histoire = categories.GetCategorieViaNom("Histoire");
        Categorie musique = categories.GetCategorieViaNom("Musique");
        Categorie nature = categories.GetCategorieViaNom("Nature");
        
        // Serialize les cartes du jeu
        Cartes cartesSerializes = XMLUtils<Cartes>.Deserialization(Path.Combine(basePath, @"..\..\..\xml\Cartes.xml"));
        
        // Set la catégorie des cartes grâce à leur id et mélange les réponses
        foreach (var carte in cartesSerializes.ListeCartes)
        {
            carte.Categorie = categories.GetCategorieViaId(carte.CategorieId);
            carte.MelangerReponses();
        }
        
        // Créé la liste des cases
        var cases = new List<Case>
        {
            // Ligne du haut
            new Case(new Vector2(30, 200), 130, TypeCase.VIDE, _textureCase),
            new Case(new Vector2(160, 200), 130, TypeCase.VIDE, _textureCase),
            new Case(new Vector2(290, 200), 130, TypeCase.QUESTION, _textureCase,sport),
            new Case(new Vector2(420, 200), 130, TypeCase.CHANCE, _textureCase, jeuxVideo),
            new Case(new Vector2(550, 200), 130, TypeCase.VIDE, _textureCase),
            new Case(new Vector2(680, 200), 130, TypeCase.JOKER, _textureCase),
            new Case(new Vector2(810, 200), 130, TypeCase.QUESTION, _textureCase, musique),
            new Case(new Vector2(940, 200), 130, TypeCase.QUESTION, _textureCase, sport),
            new Case(new Vector2(1070, 200), 130, TypeCase.VIDE, _textureCase),

            // Colonne de droite
            new Case(new Vector2(1070, 265), 130, TypeCase.JOKER, _textureCase),
            new Case(new Vector2(1070, 330), 130, TypeCase.QUESTION, _textureCase, histoire),
            new Case(new Vector2(1070, 395), 130, TypeCase.CHANCE, _textureCase, zytho),
            new Case(new Vector2(1070, 460), 130, TypeCase.VIDE, _textureCase),
            new Case(new Vector2(1070, 525), 130, TypeCase.QUESTION, _textureCase, jeuxVideo),
            new Case(new Vector2(1070, 590), 130, TypeCase.JOKER, _textureCase),
            new Case(new Vector2(1070, 655), 130, TypeCase.QUESTION, _textureCase, nature),
            new Case(new Vector2(1070, 720), 130, TypeCase.QUESTION, _textureCase, musique),

            // Ligne du bas
            new Case(new Vector2(940, 720), 130, TypeCase.VIDE, _textureCase),
            new Case(new Vector2(810, 720), 130, TypeCase.QUESTION, _textureCase, histoire),
            new Case(new Vector2(680, 720), 130, TypeCase.CHANCE, _textureCase, nature),
            new Case(new Vector2(550, 720), 130, TypeCase.VIDE, _textureCase),
            new Case(new Vector2(420, 720), 130, TypeCase.QUESTION, _textureCase, zytho),
            new Case(new Vector2(290, 720), 130, TypeCase.JOKER, _textureCase),
            new Case(new Vector2(160, 720), 130, TypeCase.VIDE, _textureCase),
            new Case(new Vector2(30, 720), 130, TypeCase.QUESTION, _textureCase, jeuxVideo),

            // Colonne de gauche
            new Case(new Vector2(30, 655), 130, TypeCase.CHANCE, _textureCase, sport),
            new Case(new Vector2(30, 590), 130, TypeCase.VIDE, _textureCase),
            new Case(new Vector2(30, 525), 130, TypeCase.QUESTION, _textureCase, musique),
            new Case(new Vector2(30, 460), 130, TypeCase.JOKER, _textureCase),
            new Case(new Vector2(30, 395), 130, TypeCase.QUESTION, _textureCase, zytho),
            new Case(new Vector2(30, 330), 130, TypeCase.QUESTION, _textureCase, nature),
            new Case(new Vector2(30, 265), 130, TypeCase.QUESTION, _textureCase, histoire),
        };
        
        _plateau = new Plateau(_fondMenu, cases);

        _partie = new Partie(_plateau, _joueurs, cartesSerializes.ListeCartes);
    }

    /// <summary>
    /// Met à jour l'état du jeu en fonnction de son etat actuel (menu, plateau, choix de difficulté, etc...) présent dans son attribut etatJeuActif.
    /// Surveille les actions des joueurs (entrès claviers) pour le déplacement la sélection de réponse, l'avancement de case et l'utilisation de joker.
    /// Passe d'un état du jeu à un autre en fonction des actions faites par les joueurs et des régles de la partie.
    /// </summary>
    /// <param name="gameTime">temps écoulé depuis la dernière maj du jeu</param>
    protected override void Update(GameTime gameTime)
    {

        switch (etatJeuActif)
        {
            
            case EtatJeu.MENU:
                _menuPage.Update(gameTime);
               
                break;
            
            case EtatJeu.PLATEAU:
                Joueur joueurAJouer = _partie.Joueurs[_partie.TourJoueur];
                
                if (joueurAJouer.Etat == EtatJoueur.Normal)
                {
                    _phrasePrincipale = "A " + joueurAJouer.GetNom() + " de jouer, appuyez sur espace.";
                }
                else
                {
                    _phrasePrincipale = joueurAJouer.GetNom() + " c'est déplacé ! Appuyez sur espace pour répondre à la question";
                }
                
                if (joueurAJouer.Etat == EtatJoueur.ChoixDifficulte)
                {
                    etatJeuActif = EtatJeu.CHOIX_DIFFICULTE;
                    
                    // Repositionne le joueur
                    joueurAJouer.SetPosition(new Vector2(795, 850));
                }
                
                UpdateInput();

                break;
            
            case EtatJeu.CHOIX_DIFFICULTE:
                Joueur joueurAJouerD = _partie.Joueurs[_partie.TourJoueur];
                
                List<ElementInteractif> difficultes = new List<ElementInteractif>
                {
                    new ElementInteractif(new Rectangle(100, 450, 400, 200), Color.White, "Facile"),
                    new ElementInteractif(new Rectangle(600, 450, 400, 200), Color.White, "Moyen"),
                    new ElementInteractif(new Rectangle(1100, 450, 400, 200), Color.White, "Difficile")
                };

                joueurAJouerD.Update();
                
                // Check si le joueur est sure une case de difficulté
                foreach (ElementInteractif difficulte in difficultes)
                {
                    if (joueurAJouerD._Rect.Intersects(difficulte.Rectangle))
                    {
                        // Si le joueur appuie sur espace sur une de ces cases
                        if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        {
                            // Convertit la valeur de l'enum en string et compare
                            if (Enum.TryParse<Difficulte>(difficulte.Texte, true, out Difficulte difficulteChoisie))
                            {
                                // Pioche une carte random
                                _partie.PiocherCarte(joueurAJouerD.GetCase().Categorie, difficulteChoisie);

                                // Modifie l'état du joueur et de la scène
                                joueurAJouerD.Etat=EtatJoueur.ChoixReponse;
                                etatJeuActif = EtatJeu.CHOIX_REPONSE;
                                
                                // Sélectionner aléatoirement les réponses 50/50
                                _Reponses5050 = joueurAJouerD.JokerActif.Jouer5050(_partie.CartePiochee);
                                
                                // Repositionne le joueur
                                joueurAJouerD.SetPosition(new Vector2(795, 850));
                            }
                        }
                    }
                }
                
                break;
            
            case EtatJeu.CHOIX_REPONSE:
                Joueur joueurAJouerR = _partie.Joueurs[_partie.TourJoueur];
                
                _partie.Joueurs[_partie.TourJoueur].Update();
                string question = _partie.CartePiochee.Question;
                
                List<ElementInteractif> elementReponsesU = new List<ElementInteractif>();
                List<ElementInteractif> elementsJokersU = new List<ElementInteractif>();
                List<Joker> jokersJoueurU = new List<Joker>();
                
                
                int nombreCarte = 4;
                
                // POSITIONNEMENT ET SURVEILLANCE DES REPONSES EN FONCTION DE L'UTILISATION D'UN JOKER
                if (joueurAJouerR.JokerActif.getNom().Equals("50/50"))
                {
                    for (int i = 0; i < _Reponses5050.Count; i++)
                    {
                        Reponse rep = _Reponses5050[i];
                        if (_Reponses5050.Contains(rep))
                        {
                            if (_Reponses5050[0].Equals(_partie.CartePiochee.Reponses[0])) elementReponsesU.Add(new ElementInteractif(new Rectangle(320, 325, 450, 200), Color.Green, _partie.CartePiochee.Reponses[0].Texte));
                            if (_Reponses5050[0].Equals(_partie.CartePiochee.Reponses[1])) elementReponsesU.Add(new ElementInteractif(new Rectangle(900, 325, 450, 200), Color.Green, _partie.CartePiochee.Reponses[1].Texte));
                            if (_Reponses5050[0].Equals(_partie.CartePiochee.Reponses[2])) elementReponsesU.Add(new ElementInteractif(new Rectangle(320, 600, 450, 200), Color.Green, _partie.CartePiochee.Reponses[2].Texte));
                            if (_Reponses5050[0].Equals(_partie.CartePiochee.Reponses[3])) elementReponsesU.Add(new ElementInteractif(new Rectangle(900, 600, 450, 200), Color.Green, _partie.CartePiochee.Reponses[3].Texte));
                                
                            if (_Reponses5050[1].Equals(_partie.CartePiochee.Reponses[0])) elementReponsesU.Add(new ElementInteractif(new Rectangle(320, 325, 450, 200), Color.Red, _partie.CartePiochee.Reponses[0].Texte));
                            if (_Reponses5050[1].Equals(_partie.CartePiochee.Reponses[1])) elementReponsesU.Add(new ElementInteractif(new Rectangle(900, 325, 450, 200), Color.Red, _partie.CartePiochee.Reponses[1].Texte));
                            if (_Reponses5050[1].Equals(_partie.CartePiochee.Reponses[2])) elementReponsesU.Add(new ElementInteractif(new Rectangle(320, 600, 450, 200), Color.Red, _partie.CartePiochee.Reponses[2].Texte));
                            if (_Reponses5050[1].Equals(_partie.CartePiochee.Reponses[3])) elementReponsesU.Add(new ElementInteractif(new Rectangle(900, 600, 450, 200), Color.Red, _partie.CartePiochee.Reponses[3].Texte));
                        }
                    }

                    nombreCarte = 2;
                }
                else if (joueurAJouerR.JokerActif.getNom().Equals("Relance de question"))
                {
                    elementReponsesU.Add(new ElementInteractif(new Rectangle(320, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[0].Texte)); 
                    elementReponsesU.Add(new ElementInteractif(new Rectangle(900, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[1].Texte));
                    elementReponsesU.Add(new ElementInteractif(new Rectangle(320, 575, 450, 200), Color.White, _partie.CartePiochee.Reponses[2].Texte));
                    elementReponsesU.Add(new ElementInteractif(new Rectangle(900, 575, 450, 200), Color.White, _partie.CartePiochee.Reponses[3].Texte));
                }
                else
                {
                    jokersJoueurU = joueurAJouerR.GetJokers().GroupBy(j => j.getNom()).Select(group => group.First()).ToList();
                    
                    elementReponsesU.Add(new ElementInteractif(new Rectangle(320, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[0].Texte)); 
                    elementReponsesU.Add(new ElementInteractif(new Rectangle(900, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[1].Texte));
                    elementReponsesU.Add(new ElementInteractif(new Rectangle(320, 575, 450, 200), Color.White, _partie.CartePiochee.Reponses[2].Texte));
                    elementReponsesU.Add(new ElementInteractif(new Rectangle(900, 575, 450, 200), Color.White, _partie.CartePiochee.Reponses[3].Texte));
                }
                
                // GESTION DES REPONSES
                int numReponse = 0;
                
                // Vérifie la validation des réponses
                foreach (ElementInteractif reponse in elementReponsesU)
                {
                    if (joueurAJouerR._Rect.Intersects(reponse.Rectangle))
                    {
                        // Fait jouer la réponse au joueur et enleve son jokerActif
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            joueurAJouerR.JouerReponse(_partie.CartePiochee.Reponses[numReponse]);
                            
                            for (int i = 0; i < nombreCarte; i++)
                            {
                                if (_partie.CartePiochee.EstCorrecte(_partie.CartePiochee.Reponses[i]))
                                {
                                    elementReponsesU[i].Couleur = _couleurReponseValide;
                                }
                                else
                                {
                                    elementReponsesU[i].Couleur = _couleurReponseNonValide;
                                }
                            }
                            
                            // Si le joueur répond juste à la question
                            joueurAJouerR.JokerActif = new Joker("", "");
                            etatJeuActif = EtatJeu.REPONSE;
                            
                            // Stocke les réponses avec leurs couleurs
                            _elementReponsesChoisies = elementReponsesU; 
                        }
                    }
                    numReponse++;
                }
                
                // GESTION DES JOKERS
                // Recupere une seule instance de chaque joker différent
                List<Joker> jokersJoueur = joueurAJouerR.GetJokers().GroupBy(j => j.getNom()).Select(group => group.First()).ToList();

                if (jokersJoueur.Count == 1)
                {
                    elementsJokersU.Add(new ElementInteractif(new Rectangle(1200, 800, 200, 75), Color.White, jokersJoueur[0].getNom()));
                }
                else if(jokersJoueur.Count == 2)
                {
                    elementsJokersU.Add(new ElementInteractif(new Rectangle(1120, 800, 200, 75), Color.White, jokersJoueur[0].getNom()));
                    elementsJokersU.Add(new ElementInteractif(new Rectangle(1390, 800, 200, 75), Color.White, jokersJoueur[1].getNom()));
                }

                int numJoker = 0;
                // Vérifie l'utilisation des joker
                foreach (ElementInteractif elementJoker in elementsJokersU)
                {
                    if (joueurAJouerR._Rect.Intersects(elementJoker.Rectangle))
                    {
                        // Utilisation du joker
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_ancienEtatClavier.IsKeyDown(Keys.Enter))
                        {
                            joueurAJouerR.JouerJoker(_partie, jokersJoueur[numJoker]);
                        }
                    }
                    numJoker ++;
                }
                _ancienEtatClavier = Keyboard.GetState();
                break;
            
            case EtatJeu.REPONSE:
                _phrasePrincipale = "Appuyez sur ENTREE pour continuer";
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_ancienEtatClavier.IsKeyDown(Keys.Enter))
                {
                    etatJeuActif = EtatJeu.TOUR_TERMINE;
                }
                _ancienEtatClavier = Keyboard.GetState();
                
                break;
            
            case EtatJeu.TOUR_TERMINE:
                Joueur joueurAJouerT = _partie.Joueurs[_partie.TourJoueur];

                if (_partie.EstTerminee())
                {
                    etatJeuActif = EtatJeu.FIN_PARTIE;
                    _phrasePrincipale = "";
                }
                else
                {
                    etatJeuActif = EtatJeu.PLATEAU;
                    joueurAJouerT.SetPositionSurCase();
                    
                    // Si le joueur a répondu juste à une case chance, il rejoue, sinon c'est au joueur suivant
                    if (joueurAJouerT.Etat != EtatJoueur.Rejouer)
                    {
                        _partie.TourJoueur = (_partie.TourJoueur + 1) % _joueurs.Count;
                    }
                    else
                    {
                        joueurAJouerT.Etat = EtatJoueur.Normal;
                    }
                }
                break;
            
            case EtatJeu.FIN_PARTIE:
                UpdateFinPartieInput();
                break;
            
        }
        base.Update(gameTime);
    }
    
    /// <summary>
    /// Vérifie  si la touche espace pressée et effectue des actions en fonction de l'état du joueur.
    /// Empêche les actions multiples dues à un appui prolongé sur la touche.
    /// </summary>
    private void UpdateInput()
    {
        Joueur joueurAJouer = _partie.Joueurs[_partie.TourJoueur];

        KeyboardState nouvelEtat = Keyboard.GetState();
        if (nouvelEtat.IsKeyDown(Keys.Space))
        {
            if (!_ancienEtatClavier.IsKeyDown(Keys.Space))
            {
                if (joueurAJouer.Etat == EtatJoueur.Normal)
                {
                    _partie.JouerTour();
                }

                else if (joueurAJouer.Etat == EtatJoueur.AttenteConfirmation)
                {
                    joueurAJouer.ActiverChoixDifficulte();
                }
            }
        }
        _ancienEtatClavier = nouvelEtat;
    }
    
    private void UpdateFinPartieInput()
    {
        MouseState mouseState = Mouse.GetState();
        Rectangle retourMenuRect = new Rectangle(650, 700, 300, 100);

        if (retourMenuRect.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Released && _ancienEtatSouris.LeftButton == ButtonState.Pressed)
        {
            // Permet de retourner au menu principal
            RetournerAuMenu();
        }

        _ancienEtatSouris = mouseState;
    }
    
    /// <summary>
    /// Gére l'affichage des éléments de la partie en fonction de l'état du jeu
    /// - Si l'état est MENU, dessine le menu principal
    /// - Si l'état est PLATEAU, dessine le plateau avec la position des joueurs et leurs informations
    /// - Si l'état est CHOIX_DIFFICULTE, affiche les trois options de difficulté (facile, moyen et difficile)
    /// - Si l'état est CHOIX_REPONSE ou REPONSE, affiche les réponses à la carte et éventuellement les jokers que le joueur peut utiliser
    /// - Si l'état est FIN_PARTIE, affiche le tableau des scores
    /// Affiche dans tous les cas le message principale du jeu au centre haut de l'écran
    /// </summary>
    /// <param name="gameTime">temps écoulé depuis la dernière maj du jeu</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        
        switch (etatJeuActif)
        {
            case EtatJeu.MENU:
                _menuPage.Draw(_spriteBatch);
                
                break;
            
            case EtatJeu.PLATEAU:
                _plateau.Draw(_spriteBatch);
                
                _spriteBatch.Draw(_textureNeutre, new Rectangle(1225, 0, 375, 900), Color.White);
                _spriteBatch.DrawString(_fontTexte, "Etat de la partie", new Vector2(1315,50), Color.Black);
                
                foreach (var joueur in _joueurs) 
                {
                    joueur.Draw(_spriteBatch);
                }
               
                int nbJoueurs = _partie.Joueurs.Count;

                float positionInfoJoueurY = 200;
                
                // AFFICHAGE DES INFOS DE CHAQUE JOUEUR SUR LE COTE DROIT DU PLATEAU
                for (int i = 0; i < nbJoueurs; i++)
                { 
                    Joueur joueur = _partie.Joueurs[i];
                    Joueur joueurTemp = new Joueur(joueur.GetNom(), joueur._Texture, joueur.GetPosition(), joueur.GetCase());
                    joueurTemp.SetPosition(new Vector2(1280,positionInfoJoueurY));
                    joueurTemp.Draw(_spriteBatch);
                    
                    _spriteBatch.DrawString(_fontTexte, joueurTemp.GetNom(), new Vector2(1310,positionInfoJoueurY-10), Color.Black);
                    _spriteBatch.DrawString(_fontTexte, "Nombre de camembert : " + _partie.Joueurs[i].Score, new Vector2(1250,positionInfoJoueurY +30), Color.Black);
                    _spriteBatch.DrawString(_fontTexte, "Jokers : ", new Vector2(1250,positionInfoJoueurY +50), Color.Black);

                    // Si le joueur a un joker, affiche tous ses différents jokers
                    if (_partie.Joueurs[i].GetJokers().Count > 0)
                    {

                        List <Joker> jokersTest = _partie.Joueurs[i].GetJokers();

                        // Recupere une seule instance de chaque joker différent
                        List<Joker> jokersUnique = _partie.Joueurs[i].GetJokers().GroupBy(j => j.getNom()).Select(group => group.First()).ToList();
                        
                        
                        int decalageTexteY = 1;
                        foreach (Joker joker in jokersUnique)
                        {
                            // Compte combien de fois le joueur a se joker
                            int nbJokers = _partie.Joueurs[i].GetJokers().Count(j => j.getNom() == joker.getNom());
                            _spriteBatch.DrawString(_fontTexte, "- " + joker.getNom() + " : " + nbJokers, new Vector2(1250,positionInfoJoueurY +50 + 20*decalageTexteY ), Color.Black);
                            decalageTexteY++;
                        }
                    }
                    else
                    {
                        _spriteBatch.DrawString(_fontTexte, " - aucun ", new Vector2(1250,positionInfoJoueurY +70), Color.Black);
                    }
                    
                    positionInfoJoueurY += 170;
                }
                break;
            
            case EtatJeu.CHOIX_DIFFICULTE:
                Joueur joueurAJouerD = _partie.Joueurs[_partie.TourJoueur];
                
                _spriteBatch.Draw(_fondGeneral, new Rectangle(0, 0, 1600, 900), Color.White);

                string titreCategorie = "Catégorie : " + joueurAJouerD.GetCase().Categorie.Nom;
                string phraseChoixDifficulte = "Veuillez choisir le niveau de difficulté de votre question";
                _spriteBatch.DrawString(_fontTitre, titreCategorie, new Vector2(_graphics.PreferredBackBufferWidth/2 - (_fontTitre.MeasureString(titreCategorie).X / 2), 150), _textCouleur);
                _spriteBatch.DrawString(_fontTexte, phraseChoixDifficulte, new Vector2(_graphics.PreferredBackBufferWidth/2 - (_fontTexte.MeasureString(phraseChoixDifficulte).X / 2), 250), _textCouleur);

                _phrasePrincipale = "";
                
                List<ElementInteractif> difficultes = new List<ElementInteractif>
                {
                    new ElementInteractif(new Rectangle(100, 450, 400, 200), Color.White, "Facile"),
                    new ElementInteractif(new Rectangle(600, 450, 400, 200), Color.White, "Moyen"),
                    new ElementInteractif(new Rectangle(1100, 450, 400, 200), Color.White, "Difficile")
                };
                
                foreach (ElementInteractif difficulte in difficultes)
                {
                    difficulte.Draw(_spriteBatch, _textureCase, _fontReponse, Color.Black);
                    
                    // Vérifie si le joueur est au dessus d'un choix de difficulte
                    if (joueurAJouerD._Rect.Intersects(difficulte.Rectangle))
                    {
                        // Place une indication au dessus du rectangle
                        Vector2 messagePosition = new Vector2(
                            difficulte.Rectangle.X + (difficulte.Rectangle.Width / 2) - (_fontInstruction.MeasureString("Appuyez sur ESPACE").X / 2),
                            difficulte.Rectangle.Y - 20
                        );
                        _spriteBatch.DrawString(_fontInstruction, "Appuyez sur ESPACE", messagePosition, _textCouleur);
                    }
                }
                
                joueurAJouerD.Draw(_spriteBatch);
                
                break;
            
            case EtatJeu.CHOIX_REPONSE:
            case EtatJeu.REPONSE:
                Joueur joueurAJouerR = _partie.Joueurs[_partie.TourJoueur];
                
                _spriteBatch.Draw(_fondGeneral, new Rectangle(0, 0, 1600, 900), Color.White);
                string question = _partie.CartePiochee.Question;
                string titre = "Catégorie : " + joueurAJouerR.GetCase().Categorie.Nom;
                
                _spriteBatch.DrawString(_fontTitre, titre, new Vector2(_graphics.PreferredBackBufferWidth/2 - (_fontTitre.MeasureString(titre).X / 2), 150), _textCouleur);
                _spriteBatch.DrawString(_fontTexte, question , new Vector2(_graphics.PreferredBackBufferWidth/2 - (_fontTexte.MeasureString(question).X / 2), 250), _textCouleur);

                _phrasePrincipale = "";
                
                List<ElementInteractif> elementReponses = new List<ElementInteractif>();
                List<ElementInteractif> elementsJokers = new List<ElementInteractif>();
                List<Joker> jokersJoueur = new List<Joker>();


                if (etatJeuActif == EtatJeu.CHOIX_REPONSE)
                {
                   // POSITIONNEMENT ET SURVEILLANCE DES REPONSES
                    if (joueurAJouerR.JokerActif.getNom().Equals("50/50"))
                    {
                        for (int i = 0; i < _Reponses5050.Count; i++)
                        {
                            Reponse rep = _Reponses5050[i];
                            if (_Reponses5050.Contains(rep))
                            {
                                if (_Reponses5050[0].Equals(_partie.CartePiochee.Reponses[0])) elementReponses.Add(new ElementInteractif(new Rectangle(320, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[0].Texte));
                                if (_Reponses5050[0].Equals(_partie.CartePiochee.Reponses[1])) elementReponses.Add(new ElementInteractif(new Rectangle(900, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[1].Texte));
                                if (_Reponses5050[0].Equals(_partie.CartePiochee.Reponses[2])) elementReponses.Add(new ElementInteractif(new Rectangle(320, 600, 450, 200), Color.White, _partie.CartePiochee.Reponses[2].Texte));
                                if (_Reponses5050[0].Equals(_partie.CartePiochee.Reponses[3])) elementReponses.Add(new ElementInteractif(new Rectangle(900, 600, 450, 200), Color.White, _partie.CartePiochee.Reponses[3].Texte));
                                
                                if (_Reponses5050[1].Equals(_partie.CartePiochee.Reponses[0])) elementReponses.Add(new ElementInteractif(new Rectangle(320, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[0].Texte));
                                if (_Reponses5050[1].Equals(_partie.CartePiochee.Reponses[1])) elementReponses.Add(new ElementInteractif(new Rectangle(900, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[1].Texte));
                                if (_Reponses5050[1].Equals(_partie.CartePiochee.Reponses[2])) elementReponses.Add(new ElementInteractif(new Rectangle(320, 600, 450, 200), Color.White, _partie.CartePiochee.Reponses[2].Texte));
                                if (_Reponses5050[1].Equals(_partie.CartePiochee.Reponses[3])) elementReponses.Add(new ElementInteractif(new Rectangle(900, 600, 450, 200), Color.White, _partie.CartePiochee.Reponses[3].Texte));
                            }
                        }
                    }
                    else if (joueurAJouerR.JokerActif.getNom().Equals("Relance de question"))
                    {
                        elementReponses.Add(new ElementInteractif(new Rectangle(320, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[0].Texte)); 
                        elementReponses.Add(new ElementInteractif(new Rectangle(900, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[1].Texte));
                        elementReponses.Add(new ElementInteractif(new Rectangle(320, 575, 450, 200), Color.White, _partie.CartePiochee.Reponses[2].Texte));
                        elementReponses.Add(new ElementInteractif(new Rectangle(900, 575, 450, 200), Color.White, _partie.CartePiochee.Reponses[3].Texte));
                    }
                    else
                    {
                        jokersJoueur = joueurAJouerR.GetJokers().GroupBy(j => j.getNom()).Select(group => group.First()).ToList();
                        
                        elementReponses.Add(new ElementInteractif(new Rectangle(320, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[0].Texte)); 
                        elementReponses.Add(new ElementInteractif(new Rectangle(900, 325, 450, 200), Color.White, _partie.CartePiochee.Reponses[1].Texte));
                        elementReponses.Add(new ElementInteractif(new Rectangle(320, 575, 450, 200), Color.White, _partie.CartePiochee.Reponses[2].Texte));
                        elementReponses.Add(new ElementInteractif(new Rectangle(900, 575, 450, 200), Color.White, _partie.CartePiochee.Reponses[3].Texte));
                    }
                }
                else
                {
                    elementReponses = _elementReponsesChoisies;
                }

                foreach (ElementInteractif reponse in elementReponses)
                {
                    reponse.Draw(_spriteBatch, _textureCase, _fontReponse, Color.Black);
                    
                    // Vérifie si le joueur est au dessus d'un choix de réponse
                    if (joueurAJouerR._Rect.Intersects(reponse.Rectangle))
                    {
                        // Place une indication au dessus du rectangle
                        Vector2 messagePosition = new Vector2(
                            reponse.Rectangle.X + (reponse.Rectangle.Width / 2) - (_fontInstruction.MeasureString("Appuyez sur ENTREE").X / 2),
                            reponse.Rectangle.Y - 20
                        );
                        _spriteBatch.DrawString(_fontInstruction, "Appuyez sur ENTREE", messagePosition, _textCouleur);
                    }
                }
                
                // POSITIONNEMENT ET SURVEILLANCE DES JOKERS
                // Recupere une seule instance de chaque joker différent
                
                if (jokersJoueur.Count == 1)
                {
                    elementsJokers.Add(new ElementInteractif(new Rectangle(1200, 800, 200, 75), Color.White, jokersJoueur[0].getNom()));
                }
                else if(jokersJoueur.Count == 2)
                {
                    elementsJokers.Add(new ElementInteractif(new Rectangle(1120, 800, 200, 75), Color.White, jokersJoueur[0].getNom()));
                    elementsJokers.Add(new ElementInteractif(new Rectangle(1390, 800, 200, 75), Color.White, jokersJoueur[1].getNom()));
                }

                foreach (ElementInteractif elementJoker in elementsJokers)
                {
                    elementJoker.Draw(_spriteBatch, _textureCase, _fontCase, Color.Black);
                    if (joueurAJouerR._Rect.Intersects(elementJoker.Rectangle))
                    {
                        // Place une indication au dessus du rectangle
                        Vector2 messagePosition = new Vector2(
                            elementJoker.Rectangle.X + (elementJoker.Rectangle.Width / 2) - (_fontInstruction.MeasureString("Appuyez sur ENTREE").X / 2),
                            elementJoker.Rectangle.Y - 20
                        );
                        _spriteBatch.DrawString(_fontInstruction, "Appuyez sur ENTREE", messagePosition, _textCouleur);
                    }
                }
                
                joueurAJouerR.Draw(_spriteBatch);
                break;
            
            case EtatJeu.FIN_PARTIE:
                _tableauDesScores.Draw(_spriteBatch); 
                
                break;
        }
        

        // Affiche le message principal au milieu du plateau
        _spriteBatch.DrawString(_fontInstruction, _phrasePrincipale, new Vector2(400, 500), _textCouleur);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
    
    private void LancerPartie(object sender, EventArgs e)
    {
        var (nombreJoueur, nomsJoueurs) = _menuPage.GetDonneesJoueurs();
        _joueurs.Clear();
        
        int espace = 0;
        for (int i = 0; i < nombreJoueur; i++)
        {
            Joueur joueur = new Joueur(nomsJoueurs[i], _textureJoueurs[i], new Vector2(60 + espace, 240), _plateau.GetCase(0));
            _joueurs.Add(joueur);
            espace += 20;
        }
        
        etatJeuActif = EtatJeu.PLATEAU;
    }
    
    private void TableauDesScores_RetourMenuClicked(object sender, EventArgs e)
    {
        RetournerAuMenu();
    }
    
    private void RetournerAuMenu()
    {
        etatJeuActif = EtatJeu.MENU;
        // Réinitialise les éléments nécessaires pour le menu principal
        _menuPage = new MenuPage(_fondMenu, _fontTitre, _fontTexte, _textureCase, _fontInstruction);
        _menuPage.LancerPartie += LancerPartie;
        _phrasePrincipale = "";
    }

}