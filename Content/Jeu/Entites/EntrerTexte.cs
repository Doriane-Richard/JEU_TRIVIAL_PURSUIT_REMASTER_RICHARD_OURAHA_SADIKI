namespace Trivial_Pursuit.Jeu.Entites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class EntrerTexte
{
    private Texture2D _texture;
    private SpriteFont _font;
    private Vector2 _position;
    private int _width;
    private string _text;
    private KeyboardState _oldState;
    private bool _isActive;
    private InputType _inputType;

    /// <summary>
    /// Enumération pour spécifier le type d'entrée autorisé dans la zone de texte
    /// </summary>
    public enum InputType
    {
        Number,
        Text
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="texture">Texture utilisée pour dessiner la zone de texte</param>
    /// <param name="font">Police utilisée pour dessiner le texte</param>
    /// <param name="position">Position de la zone de texte</param>
    /// <param name="width">Largeur de la zone de texte</param>
    /// <param name="inputType">Type d'entrée (Numéro ou Texte)</param>
    public EntrerTexte(Texture2D texture, SpriteFont font, Vector2 position, int width, InputType inputType)
    {
        _texture = texture;
        _font = font;
        _position = position;
        _width = width;
        _text = string.Empty;
        _isActive = false;
        _inputType = inputType;
    }

    /// <summary>
    /// Mise à jour de la zone de texte, vérifie les touches pressées et met à jour le text
    /// </summary>
    public void Update(GameTime gameTime)
    {
        if (_isActive)
        {
            KeyboardState newState = Keyboard.GetState();
            Keys[] pressedKeys = newState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (_oldState.IsKeyUp(key)) // Check if the key was just pressed
                {
                    if (_inputType == InputType.Number && Char.IsDigit((char)key)) // Check if the key is a digit
                    {
                        // If it's a digit, append it to the text string
                        _text += (char)key;
                    }
                    else if (_inputType == InputType.Text && Char.IsLetterOrDigit((char)key)) // Check if the key is a letter or digit
                    {
                        // If it's a letter or digit, append it to the text string
                        _text += (char)key;
                    }
                    else if (key == Keys.Back) // Check if the key is backspace
                    {
                        // If it's backspace, remove the last character from the text string
                        if (_text.Length > 0)
                            _text = _text.Remove(_text.Length - 1);
                    }
                }
            }

            _oldState = newState;
        }
    }

    /// <summary>
    /// Dessine la zone de texte et le texte entré dans la zone
    /// </summary>
    /// <param name="spriteBatch">Objet SpriteBatch pour dessiner les éléments à l'écran</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, _width, 50), Color.White);
        spriteBatch.DrawString(_font, _text, _position + new Vector2(10, 10), Color.Black); // Add space around the text
    }

    public string GetText()
    {
        return _text;
    }

    /// <summary>
    /// Gère les clics de la souris pour savoir si l'utilisateur a cliqué sur la zone de texte
    /// </summary>
    public void HandleClick(MouseState mouseState)
    {
        var mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
        var textBoxRectangle = new Rectangle((int)_position.X, (int)_position.Y, _width, 50);

        if (mouseRectangle.Intersects(textBoxRectangle))
        {
            _isActive = true;
        }
        else
        {
            _isActive = false;
        }
    }
}
