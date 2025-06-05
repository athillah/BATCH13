using Domino.Interfaces;
namespace Domino.Models;

public class Deck : IDeck
{
    // private bool emptyDeck
    public List<ICard> Cards { get; set; }
    public Deck() => Cards = new List<ICard>();
    public bool IsEmpty() => Cards.Count == 0;
}