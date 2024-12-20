using System.Drawing;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Microsoft.Xna.Framework.Graphics;
using Trivial_Pursuit.Jeu.Enumeration;

namespace Trivial_Pursuit.Jeu.Entites;


// Une case contient un Type (Chance, joker, question ou vide), un fond (avec couleur et texte) et une catégorie dans le cas ou elle contient une questions
public class Case : Sprite
{
    public TypeCase Type { get; private set; } 
    private static SpriteFont _fontCase; // Constante initialisée en debut de programme
    public  Categorie Categorie { get; private set; }
    
    /// <summary>
    /// Initialise une case avec une position, une taille, un son type sa texture et sa catégorie
    /// Si la case n'a pas de catégorie spécifiée, elle aura la catégorie de vase c'est à dire sans catégorie
    /// </summary>
    /// <param name="position">La position de la case sur le plateau</param>
    /// <param name="taille">La taille de la case pour son sprite</param>
    /// <param name="typeCase">Le type de la case (JOKER, CHANCE, QUESTION, VIDE).</param>
    /// <param name="texture">La texture de la case pour son sprite</param>
    /// <param name="categorie">La catégorie opetionnelle de la case</param>
    public Case(Vector2 position, int taille, TypeCase typeCase, Texture2D texture, Categorie categorie = null) 
        : base(texture, position, taille, (categorie ?? new Categorie()).Couleur) // constructeur de Sprite
    {
        Type = typeCase;
        Categorie = categorie ?? new Categorie(); // Si categorie pas fournit, categorie par défaut
    }
    
    /// <summary>
    /// set la police utilisée pour la case
    /// </summary>
    /// <param name="font">font de la case</param>
    public static void SetFontCase(SpriteFont font)
    {
        _fontCase = font;
    }
    
    public Vector2 GetPosition()
    {
        return _position;
    }

    /// <summary>
    /// Redéfinit la méthode Draw de sprite pour que le rectangle soit au bon format et dessine la case avec son potentiel texte à l'intérieur
    /// </summary>
    /// <param name="spriteBatch">Utilisé pour dessiner la case</param>
    public new void Draw(SpriteBatch spriteBatch)
    {
        // Calcule les dimentions de l'image
        int longueur = _Size;
        int largeur = (int)((float)_texture.Height / _texture.Width * longueur);
        string text = "";

        // Créé un rectangle avec les dimentions calculées
        Rectangle rectangle = new Rectangle((int)_position.X, (int)_position.Y, longueur, largeur);
        
        spriteBatch.Draw(_texture, rectangle, Categorie.Couleur);
        
        if (Type == TypeCase.JOKER)
        {
            text = "joker";
            // Calcul la taille du texte
            Vector2 textSize = _fontCase.MeasureString(text);

            // Adapte la position du texte pour le centrer
            Vector2 textPosition = new Vector2(
                _position.X + (longueur / 2) - (textSize.X / 2),
                _position.Y + (largeur / 2) - (textSize.Y / 2)
            );
            
            spriteBatch.DrawString(_fontCase, text, textPosition, Color.Black); // Dessine le texte
        }
        
        else if (Type == TypeCase.CHANCE)
        {
            text = "chance";
            // Calcul la taille du texte
            Vector2 textSize = _fontCase.MeasureString(text);

            // Adapte la position du texte pour le centrer
            Vector2 textPosition = new Vector2(
                _position.X + (longueur / 2) - (textSize.X / 2),
                _position.Y + (largeur / 2) - (textSize.Y / 2)
            );
            
            spriteBatch.DrawString(_fontCase, text, textPosition, Color.Black); // Dessine le texte
        }
        
        // Case de départ
        else if (_position.X == 30 && _position.Y == 200)
        {
            text = "Depart";
            // Calcul la taille du texte
            Vector2 textSize = _fontCase.MeasureString(text);

            // Adapte la position du texte pour le centrer
            Vector2 textPosition = new Vector2(
                _position.X + (longueur / 2) - (textSize.X / 2),
                _position.Y + (largeur / 2) - (textSize.Y / 2)
            );
            
            spriteBatch.DrawString(_fontCase, text, textPosition, Color.Black); // Dessine le texte
        }
    }
}
    