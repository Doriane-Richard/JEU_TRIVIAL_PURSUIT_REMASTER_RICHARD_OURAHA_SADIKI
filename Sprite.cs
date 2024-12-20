using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


/// <summary>
/// Classe d'un sprite avec une texture, une position, une taille, et une couleur !
/// </summary>
public class Sprite
{
    protected Texture2D _texture;
    protected Vector2 _position;
    private int _size = 100;
    private static readonly int _sizeMin = 10;
    private Color _color = Color.White;
    
    
    public Texture2D _Texture { get => _texture; init => _texture = value; }
    public int _Size { get => _size; set => _size = value >= _sizeMin ? value : _sizeMin; }
    public Rectangle _Rect { get => new Rectangle((int)_position.X, (int)_position.Y, _size, _size); }

    /// <summary>
    /// Constructeur du Sprite
    /// Initialise une nouvelle instance de Sprite avec une texture, une position, une taille et une couleur, la couleur par défaut étant blanche
    /// </summary>
    /// <param name="texture">La texture du sprite</param>
    /// <param name="position">Sa position (X et Y)</param>
    /// <param name="size">Sa taille</param>
    /// <param name="color">La couleur du sprite blanche par défaut si non donnée</param>
    public Sprite(Texture2D texture, Vector2 position, int size, Color color = default)
    {
        _Texture = texture;
        _position = position;
        _Size = size;
        _color = color == default ? Color.White : color; // si couleur par défaut alors blanc sinon celle donnée
    }

    /// <summary>
    /// Dessine le sprite
    /// </summary>
    /// <param name="spriteBatch">Permet de dessiner le sprite</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        spriteBatch.Draw(
            _texture, // Texture2D
            _Rect,    // Rectangle destinationRectangle
            null,     // Nullable<Rectangle> sourceRectangle
            _color,   // Color
            0.0f,     // float rotation
            origin,   // Vector2 origin
            SpriteEffects.None, // SpriteEffects effects
            0f        // float layerDepth
        );
    }
}