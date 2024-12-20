namespace Trivial_Pursuit.Jeu.Enumeration;

/// <summary>
/// Enum�ration des diff�rents types de cases
/// </summary>
public enum TypeCase
{
    /// <summary>
    /// Case sans question ou �v�nement particulier
    /// Lorsqu'un joueur arrive dessus, le joueur suivant joue
    /// </summary>
    VIDE,

    /// <summary>
    /// Case contenant une question
    /// Lorsque le joueur arrive dessus il doit choisir la difficult� de sa question pour r�pondre � la question
    /// </summary>
    QUESTION,

    /// <summary>
    /// Case procurant un joker au joueur entre "50/50" et "Relance"
    /// Le joueur n'a pas a r�pondre � une question
    /// </summary>
    JOKER,

    /// <summary>
    /// Case question avec la particularit� qu'elle permet au joueur de jouer un nouveau tour s'il r�pond juste � la question
    /// </summary>
    CHANCE
}