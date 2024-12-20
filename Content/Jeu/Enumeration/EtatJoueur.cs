namespace Trivial_Pursuit.Jeu.Enumeration;

/// <summary>
/// Diff�rents �tats d'un joueur au cours d'une partie
/// </summary>
public enum EtatJoueur
{
    /// <summary>
    /// Lorsqu'il est sur le plateau o� � sa cr�ation
    /// </summary>
    Normal,

    /// <summary>
    /// Lorsqu'il attend une intervention de l'utilisateur pour passer � l'�tape suivante
    /// </summary>
    AttenteConfirmation,

    /// <summary>
    /// Lorsque le joueur doit choisir une difficult�
    /// </summary>
    ChoixDifficulte,

    /// <summary>
    /// Lorsque le joueur doit choisir une r�ponse
    /// </summary>
    ChoixReponse,

    /// <summary>
    /// Lorsque le joueur vient de r�pondre juste � une case chanse
    /// </summary>
    Rejouer
}