namespace Domino.Interfaces;

public class Deck : IDeck
{
    private bool _emptyDeck;
    public List<ICard> Cards { get; set; }
    public Deck()
    {
        Cards= new List<ICard>();
    }
    //
    public bool IsEmpty()
    {
        return _emptyDeck;
    }
}