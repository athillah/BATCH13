namespace Domino.Interfaces;

public interface IDeck
{
    public List<ICard> Cards { get; set; }
    public bool IsEmpty();

}