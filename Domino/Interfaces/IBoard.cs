using System.ComponentModel.DataAnnotations;
using Domino.Enums;

namespace Domino.Interfaces;

public interface IBoard
{
    List<ICard> PlayedCards { get; set; }
}