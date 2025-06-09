using System.ComponentModel.DataAnnotations;
using Domino.Interfaces;
using Domino.Enums;

namespace Domino.Models;

public class Board : IBoard
{
    public List<ICard> PlayedCards { get; set; }
    public Board() => PlayedCards = new List<ICard>();
}