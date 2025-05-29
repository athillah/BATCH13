namespace Domino.Interfaces;

public class Deck : IDeck
{
    public List<ICard> Cards { get; set; }
    public Deck()
    {
        Cards= new List<ICard>();
    }
    public bool IsEmpty()
    {
        return Cards.Count == 0;
    }
}