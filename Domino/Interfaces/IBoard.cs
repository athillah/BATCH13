using System.ComponentModel.DataAnnotations;
using Domino.Enumerations;

namespace Domino.Interfaces;

public interface IBoard
{
    List<ICard> PlayedCards { get; set; }
    void UpdateBoard(ICard card, Side side);
}