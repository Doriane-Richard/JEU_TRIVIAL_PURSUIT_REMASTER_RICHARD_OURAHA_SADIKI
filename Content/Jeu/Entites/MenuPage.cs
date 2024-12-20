using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Trivial_Pursuit.Jeu.Entites;

namespace TrivialPursuit;
public class MenuPage
{
    private Texture2D _textureFond;
    private SpriteFont _fontTitre;
    private SpriteFont _fontTexte;
    private SpriteFont _fontInstruction;
    private Bouton _boutonLancerPartie;
    private EntrerTexte _zoneTextNombreJoueur;
    private List<EntrerTexte> _inputsNomJoueur;
    private KeyboardState _ancienEtat;
    private bool _nombreJoueurValider;
    private bool _boutonLancerPartieCliquer;
    public event EventHandler LancerPartie;

    /// <summary>
    ///  Initialisation de tous les éléments nécéssaires au menu
    /// </summary>
    /// <param name="textureFond">Texture de fond du menu</param>
    /// <param name="fontTitre">Police pour le titre</param>
    /// <param name="fontTexte">Police pour le texte général</param>
    /// <param name="textureBouton">Texture pour les boutons</param>
    /// <param name="fontInstruction">Police pour les instructions</param>
    public MenuPage(Texture2D textureFond, SpriteFont fontTitre, SpriteFont fontTexte, Texture2D textureBouton, SpriteFont fontInstruction)
    {
        _textureFond = textureFond;
        _fontTitre = fontTitre;
        _fontTexte = fontTexte;
        _fontInstruction = fontInstruction;
        _boutonLancerPartie = new Bouton(textureBouton, fontTexte, "Lancer la partie", new Vector2(700, 800), 50);
        _boutonLancerPartie.Click += ClicBoutonCommencer;
        _zoneTextNombreJoueur = new EntrerTexte(textureBouton, fontTexte, new Vector2(850, 390), 200, EntrerTexte.InputType.Number);
        _inputsNomJoueur = new List<EntrerTexte>();
        _ancienEtat = Keyboard.GetState();
        _nombreJoueurValider = false;
        _boutonLancerPartieCliquer = false;
    }

    /// <summary>
    /// Met à jour l'état du menu 
    /// </summary>
    /// <param name="gameTime">Temps de jeu</param>
    public void Update(GameTime gameTime)
    {
        _boutonLancerPartie.Update(gameTime);
        _zoneTextNombreJoueur.Update(gameTime);
        foreach (var inputNomJoueur in _inputsNomJoueur)
        {
            inputNomJoueur.Update(gameTime);
        }

        int espace = 0;
        KeyboardState nouvelEtat = Keyboard.GetState();
        if (nouvelEtat.IsKeyDown(Keys.Enter))
        {
            if (!_nombreJoueurValider)
            {
                int nombreJoueurs;
                if (int.TryParse(_zoneTextNombreJoueur.GetText(), out nombreJoueurs) && nombreJoueurs > 0 && nombreJoueurs <= 4)
                {
                    _inputsNomJoueur.Clear();
                    for (int i = 0; i < nombreJoueurs; i++)
                    {
                        _inputsNomJoueur.Add(new EntrerTexte(_boutonLancerPartie._Texture, _fontTexte,
                            new Vector2(850, 450 + i * 50 + espace), 200, EntrerTexte.InputType.Text));
                        espace += 20;
                    }
                }
            }
        }
        
        if (_boutonLancerPartieCliquer)
        {
            ClicBoutonCommencer(this, EventArgs.Empty);
        }
        _ancienEtat = nouvelEtat;

        HandleClicSouris();
    }

    /// <summary>
    /// Dessine le menu à l'écran
    /// </summary>
    /// <param name="spriteBatch">Le SpriteBatch utilisé pour dessiner</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_textureFond, new Rectangle(0, 0, 1600, 900), Color.White);
        Color textColor = new Color(215, 192, 84);
        spriteBatch.DrawString(_fontTitre, "TRIVIAL PURSUIT REMASTER", new Vector2(800 - (_fontTitre.MeasureString("TRIVIAL PURSUIT REMASTER").X / 2), 10), textColor);
        
        spriteBatch.DrawString(_fontTexte, "Bienvenu très cher joueur !", new Vector2(800 - (_fontTexte.MeasureString("Bienvenu très cher joueur !").X / 2), 100), textColor);
        spriteBatch.DrawString(_fontTexte, "Dans ce jeu de culture générale vous allez devoir répondre à différentes questions en faisant  marcher vos méninges !", new Vector2(800 - (_fontTexte.MeasureString("Dans ce jeu de culture générale vous allez devoir répondre à différentes questions en faisant  marcher vos méninges !").X / 2), 150), textColor);
        spriteBatch.DrawString(_fontTexte, "Il y aura six thèmes différents : sport, histoire, musique et le nouveau thème qui fait sensation la zythologie (l'art de la bière) !", new Vector2(800 - (_fontTexte.MeasureString("Il y aura six thèmes différents : sport, histoire, musique et le nouveau thème qui fait sensation la zythologie (l'art de la bière) !").X / 2), 200), textColor);
        spriteBatch.DrawString(_fontTexte, "Amusez-vous bien !", new Vector2(800 - (_fontTexte.MeasureString("Amusez-vous bien !").X / 2), 250), textColor);
        
        
        spriteBatch.DrawString(_fontInstruction, "PS: Lorsque vous rentrer le nombre de joueur, veuillez cliqué sur l'encadré blanc et appuyer sur entrée. Le nombre de joueur est entre 1 et 4 compris.", new Vector2(800 - (_fontInstruction.MeasureString("PS: Lorsque vous rentrer le nombre de joueur, veuillez cliqué sur l'encadré blanc et appuyer sur entré. Le nombre de joueur est entre 1 et 4 compris.").X / 2), 300), textColor);
        spriteBatch.DrawString(_fontInstruction, "Pour rentrer le nom d'un joueur, cliquer sur l'encadré blanc associé et une fois fini, lancer la partie à l'aide du bouton.", new Vector2(800 - (_fontInstruction.MeasureString("Pour rentrer le nom d'un joueur, cliquer sur l'encadré blanc associé et une fois fini, lancer la partie à l'aide du bouton.").X / 2), 320), textColor);

        
        spriteBatch.DrawString(_fontTexte, "Entrer le nombre de joueur :", new Vector2(500, 390), textColor);
        _zoneTextNombreJoueur.Draw(spriteBatch);
        
        int espace = 0;
        // Afficher des zones d'entrées afin de renseigner le nom de chaque joueur
        for (int i = 0; i < _inputsNomJoueur.Count; i++)
        {
            spriteBatch.DrawString(_fontTexte, $"Entrer le nom du joueur {i + 1} :", new Vector2(500, 450 + i * 50 + espace), textColor);
            _inputsNomJoueur[i].Draw(spriteBatch);
            espace += 20;
        }
        
        _boutonLancerPartie.Draw(spriteBatch);
    }

    /// <summary>
    /// Gère l'action lorsque le bouton "Lancer la partie" est cliqué
    /// </summary>
    private void ClicBoutonCommencer(object sender, EventArgs e)
    {
        _boutonLancerPartieCliquer = true;
        // Préviens Game1.cs que la partie doit commencer
        LancerPartie?.Invoke(this, EventArgs.Empty); 
    }

    /// <summary>
    /// Gère les clics de souris sur les éléments interactifs 
    /// </summary>
    private void HandleClicSouris()
    {
        MouseState etatSouris = Mouse.GetState();
        if (etatSouris.LeftButton == ButtonState.Pressed)
        {
            _zoneTextNombreJoueur.HandleClick(etatSouris);
            foreach (var inputNomJoueur in _inputsNomJoueur)
            {
                inputNomJoueur.HandleClick(etatSouris);
            }
            _boutonLancerPartie.HandleClick(etatSouris);
        }
    }
    
    /// <summary>
    ///  Récupère les données des joueurs, c'est-à-dire le nombre de joueurs et leurs noms
    /// </summary>
    /// <returns>Un tuple contenant le nombre de joueurs et une liste des noms des joueurs</returns>
    public (int nombreJoueur, List<string> nomsJoueurs) GetDonneesJoueurs()
    {
        int nombreJoueur = 0;
        List<string> nomsJoueurs = new List<string>();
        
        
        if (int.TryParse(_zoneTextNombreJoueur.GetText(), out nombreJoueur) && nombreJoueur > 0 && nombreJoueur <= 4)
        {
            foreach (var inputNomJoueur in _inputsNomJoueur)
            {
                nomsJoueurs.Add(inputNomJoueur.GetText());
            }
        }

        return (nombreJoueur, nomsJoueurs);
    }
}
