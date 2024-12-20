using System;
using Microsoft.Xna.Framework;

namespace TrivialPursuit.Content.Jeu.Entites;

// Classe statique utilisé pour la sérialisation de couleurs, en passant du format Hexadecimal du xml au format Color
public static class CouleurConvert
{
    public static Color DeHexaACouleur(string clrHexa)
    {

        // Supprime le "#" de l'hexadecimal
        clrHexa = clrHexa.Substring(1);

        byte r = Convert.ToByte(clrHexa.Substring(0, 2), 16);
        byte g = Convert.ToByte(clrHexa.Substring(2, 2), 16);
        byte b = Convert.ToByte(clrHexa.Substring(4, 2), 16);

        return new Color(r, g, b);
    }

    
    public static string DeCouleurAHexa(Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}