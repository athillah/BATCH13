namespace Domino.Interfaces;

public interface IDeck
{
    // private bool empyDeck
    List<ICard> Cards { get; set; }
    bool IsEmpty();

}