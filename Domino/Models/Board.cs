using System.ComponentModel.DataAnnotations;
using Domino.Interfaces;
using Domino.Enumerations;

namespace Domino.Models;

public class Board : IBoard
{
    public List<ICard> PlayedCards { get; set; }
    public void UpdateBoard(ICard card, Side side)
    {
        Console.WriteLine("Update Board");
    }
    public List<ICard> GetBoard()
    {
        return _playedCards;
    }
}