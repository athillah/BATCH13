using Domino.Controllers;
using Domino.Interfaces;
using Domino.Models;
using Domino.Enumerations;
using System;

class Program
{
    static void Main()
    {
        List<IPlayer> players = new List<IPlayer>()
        {
            new Player("Alice", null),
            new Player("Bob", null)
        };

        IDeck deck = new Deck();
        IBoard board = new Board();
        GameController control = new GameController(players, deck, board);
        control.GenerateStandardDeck();
        control.Shuffle();
        Console.Write(Side.LEFT);
    }
}