namespace Domino.Interfaces;

public interface IDeck
{
    List<ICard> Cards { get; set; }
    bool IsEmpty();

}