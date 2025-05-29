using Domino.Controllers;
using Domino.Interfaces;
using Domino.Models;
using Domino.Enumerations;
using System;
using Microsoft.VisualBasic;

internal class Program
{
    private static void Main()
    {
        List<IPlayer> players =
        [
            new Player("Alice"),
            new Player("Bob"),
        ];
        IDeck deck = new Deck();
        IBoard board = new Board();
        GameController controller = new GameController(players, deck, board);
        controller.StartGame();
    }
}