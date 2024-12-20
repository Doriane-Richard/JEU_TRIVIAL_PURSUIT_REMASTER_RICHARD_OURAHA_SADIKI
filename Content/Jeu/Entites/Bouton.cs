namespace Trivial_Pursuit.Jeu.Entites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Bouton : Sprite
{
    private string _texte;
    private SpriteFont _font;
    private bool _estSurvoler;
    private MouseState _positionCourranteSouris;
    private MouseState _positionPrecedenteSouris;
    private int _hauteur;
    public Bouton(Texture2D texture, SpriteFont font, string text, Vector2 position, int hauteur)
        : base(texture, position, texture.Width, Color.White)
    {
        _font = font;
        _texte = text;
        _estSurvoler = false;
        _hauteur = hauteur;

        // Ajuste la taille du bouton en fonction de la taille du texte
        var textSize = _font.MeasureString(_texte);
        _Size = (int)textSize.X + 50;
    }

    public void Update(GameTime gameTime)
    {
        _positionPrecedenteSouris = _positionCourranteSouris;
        _positionCourranteSouris = Mouse.GetState();

        var mouseRectangle = new Rectangle(_positionCourranteSouris.X, _positionCourranteSouris.Y, 1, 1);
        _estSurvoler = false;

        if (mouseRectangle.Intersects(_Rect))
        {
            _estSurvoler = true;
            if (_positionCourranteSouris.LeftButton == ButtonState.Released && _positionPrecedenteSouris.LeftButton == ButtonState.Pressed)
            {
                OnClick();
            }
        }
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        var color = _estSurvoler ? Color.Gray : Color.White;
        var _Rect = new Rectangle((int)_position.X, (int)_position.Y, _Size, _hauteur);
        spriteBatch.Draw(_texture, _Rect, color);
        if (!string.IsNullOrEmpty(_texte))
        {
            var x = (_Rect.X + (_Rect.Width / 2)) - (_font.MeasureString(_texte).X / 2);
            var y = (_Rect.Y + (_Rect.Height / 2)) - (_font.MeasureString(_texte).Y / 2);
            spriteBatch.DrawString(_font, _texte, new Vector2(x, y), Color.Black);
        }
    }

    public event EventHandler Click;

    protected virtual void OnClick()
    {
        Click?.Invoke(this, EventArgs.Empty);
    }

    public void HandleClick(MouseState mouseState)
    {
        var mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
        if (mouseRectangle.Intersects(_Rect))
        {
            OnClick();
        }
    }

}

