using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Trivial_Pursuit.Jeu.Entites;

namespace TrivialPursuit.Content.Jeu.Entites
{
    /// Classe pour représenter le tableau des scores des joueurs
    public class TableauDesScores
    {
        private Texture2D _fondPlateau;
        private SpriteFont _fontTitre;
        private SpriteFont _fontTexte;
        private List<Joueur> _joueurs;
        private Bouton _boutonRetourMenu;
        private Color _textCouleur;
        
        /// <summary>
        /// Initialise le tableau des scores avec un fond de plateau, une police d'écriture pour le titre, une police d'écriture pour le texte,
        /// une liste de joueurs, le fond du bouton et une couleur de texte
        /// </summary>
        /// <param name="fondPlateau">La texture de fond du tableau</param>
        /// <param name="fontTitre">La police utilisée pour le titre</param>
        /// <param name="fontTexte">La police utilisée pour le texte</param>
        /// <param name="joueurs">La liste des joueurs dans la partie</param>
        /// <param name="fondBouton">Fond du bouton de retour au menu</param>
        /// <param name="textCouleur">Couleur du texte</param>
        public TableauDesScores(Texture2D fondPlateau, SpriteFont fontTitre, SpriteFont fontTexte, List<Joueur> joueurs, Texture2D fondBouton, Color textCouleur)
        {
            _fondPlateau = fondPlateau;
            _fontTitre = fontTitre;
            _fontTexte = fontTexte;
            _joueurs = joueurs;
            _textCouleur = textCouleur;
            _boutonRetourMenu = new Bouton(fondBouton, fontTexte, "Retour au menu", new Vector2(700, 700), 50);
            _boutonRetourMenu.Click += BoutonRetourMenu_Click;
        }
        
        public event EventHandler RetourMenuClicked;

        // Transmet l'information que le bouton de retour a été cliqué à TrivialPursuit.cs 
        private void BoutonRetourMenu_Click(object sender, EventArgs e)
        {
            RetourMenuClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Dessine le tableau des scores
        /// </summary>
        /// <param name="spriteBatch">Utilisé pour dessiner le tableau des score</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessiner le fond
            spriteBatch.Draw(_fondPlateau, new Rectangle(0, 0, 1600, 900), _textCouleur);

            // Dessiner le titre
            spriteBatch.DrawString(_fontTitre, "Tableau des scores", new Vector2(800 - (_fontTitre.MeasureString("Tableau des scores").X / 2), 200), _textCouleur);

            // Dessiner les en-têtes
            spriteBatch.DrawString(_fontTexte, "Nom joueur", new Vector2(650, 400), _textCouleur);
            spriteBatch.DrawString(_fontTexte, "Nombre camemberts", new Vector2(850, 400), _textCouleur);

            // Trier les joueurs par ordre décroissant en fonction du score
            var joueursTries = _joueurs.OrderByDescending(j => j.GetScore()).ToList();
            
            // Dessiner les scores des joueurs
            int espace = 0;
            int numeroJoueur = 1;
            foreach (Joueur joueur in joueursTries)
            {
                spriteBatch.DrawString(_fontTexte, $"{numeroJoueur}-", new Vector2(600, 450 + espace), _textCouleur);
                spriteBatch.DrawString(_fontTexte, joueur.GetNom(), new Vector2(650, 450 + espace), _textCouleur);
                spriteBatch.DrawString(_fontTexte, joueur.GetScore(), new Vector2(850, 450 + espace), _textCouleur);
                espace += 30;
                numeroJoueur++;
            }
            
            // Dessiner le bouton "Retourner au menu"
            _boutonRetourMenu.Draw(spriteBatch);
        }
        
        
        public void Update(GameTime gameTime)
        {
            _boutonRetourMenu.Update(gameTime);
        }
    }
}
