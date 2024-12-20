using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Trivial_Pursuit.Jeu.Entites;

namespace Trivial_Pursuit.Jeu.Entites;

/// Plateau d'une partie contenant un ensemble de cases avec une texture en fond
public class Plateau
{
    private List<Case> _cases;
    private Texture2D _background;
    
    /// <summary>
    /// initialise le plateau avec une liste de cases et la texture de fond
    /// </summary>
    /// <param name="background">la texture de fond du plateau</param>
    /// <param name="cases">la liste des cases qui présentes dans le plateau</param>
    public Plateau(Texture2D background, List<Case> cases)
    {
        _cases = cases;
        _background = background;
    }
    
    /// <summary>
    /// Retourne l'indice de la case du joueur dans la liste des cases
    /// </summary>
    /// <param name="caseJoueur">La case où le joueur se localise</param>
    /// <returns>L'indice de la case dans la liste des cases du plateau</returns>
    public int GetIndiceCase(Case caseJoueur)
    {
        return _cases.IndexOf(caseJoueur);
    }
    
    /// <summary>
    /// retourne le nombre de cases du plateau
    /// </summary>
    public int GetNombreCases()
    {
        return _cases.Count;
    }
    

    /// <summary>
    /// Recupere la case à l'indince demandé
    /// </summary>
    /// <param name="indice">Indice demandé d'une ccase</param>
    /// <returns>La case correspondant à l'indice demandé</returns>
    public Case GetCase(int indice)
    {
        return _cases[indice];
    }
    
    /// <summary>
    /// Dessine l'ensemble du plateau à savoir son fond ainsi que l'ensemnle de ses cases
    /// </summary>
    /// <param name="spriteBatch"></param>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_background, new Rectangle(0, 0, 1600, 900), Color.White);

        // Draw toute les cases
        foreach (var caseAct in _cases)
        {
            caseAct.Draw(spriteBatch);
        }
    }
}