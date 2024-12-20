namespace Trivial_Pursuit.Jeu.Enumeration;

/// <summary>
/// Diff�rents �tats du programme permettant de choisir quelle vue afficher et quelles traitements faire pour les fonctions draw et update
/// </summary>
public enum EtatJeu
{
    /// <summary>
    /// Setup de la partie (choix du nomre de joueurs et de leur pr�noms
    /// </summary>
    MENU,

    /// <summary>
    /// Lorsqu'un joueur doit se d�placer o� vient de se d�placer
    /// </summary>
    PLATEAU,

    /// <summary>
    /// Lorsqu'un joueur choisit une difficult�
    /// </summary>
    CHOIX_DIFFICULTE,

    /// <summary>
    /// Lorsqu'un joueur choisit une r�ponse
    /// </summary>
    CHOIX_REPONSE,

    /// <summary>
    /// Lorsqu'un joueur choisit une r�ponse � une question
    /// </summary>
    REPONSE,

    /// <summary>
    /// Lorsqu'un joueur vient de r�pondre � une question
    /// </summary>
    TOUR_TERMINE,

    /// <summary>
    /// Lorsqu'un joueur a gagn� la partie
    /// </summary>
    FIN_PARTIE
}