namespace TrivialPursuit.Content.Jeu.Entites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
/// Classe représentant un élément avec lequel le joueur pourra intéragir
/// Il se compose d'un rectangle, d'une couleur et d'un texte au centre du rectangle
public class ElementInteractif
{
    public Rectangle Rectangle { get; set; }
    public Color Couleur { get; set; }
    public string Texte { get; set; }

    /// <summary>
    /// Constructeur pour l'élément intéractif
    /// </summary>
    /// <param name="rectangle">Le rectangle de l'élément intéractif</param>
    /// <param name="couleur">La couleur du rectangle</param>
    /// <param name="texte">Le texte de l'élément intéractif</param>
    public ElementInteractif(Rectangle rectangle, Color couleur, string texte)
    {
        Rectangle = rectangle;
        Couleur = couleur;
        Texte = texte;
    }

    /// <summary>
    /// Dessine l'élément intéractif en prenant soin de placer le texte de l'élément intéractif au centre de son rectangle
    /// </summary>
    /// <param name="spriteBatch">Utilisé pour dessiner l'élément intéractif</param>
    /// <param name="texture">La texture du rectangle</param>
    /// <param name="font">La police du texte</param>
    /// <param name="couleur">La couleur de l'élément intéractif</param>
    public void Draw(SpriteBatch spriteBatch, Texture2D texture, SpriteFont font, Color textColor)
    {
        spriteBatch.Draw(texture, Rectangle, Couleur);
        
        Vector2 textSize = font.MeasureString(Texte);
        Vector2 textPosition = new Vector2(
            Rectangle.X + (Rectangle.Width / 2) - (textSize.X / 2),
            Rectangle.Y + (Rectangle.Height / 2) - (textSize.Y / 2)
        );

        spriteBatch.DrawString(font, Texte, textPosition, textColor);
    }
}