using System.ComponentModel.DataAnnotations;
using Domino.Enumerations;

namespace Domino.Interfaces;

public interface IBoard
{
    public List<ICard> PlayedCards { get; set; }
    public void UpdateBoard(ICard card, Side side);
    public List<ICard> GetBoard();
}