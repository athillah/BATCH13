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
            new Player("Luana"),
            new Player("Phoebe"),
        ];
        IDeck deck = new Deck();
        IBoard board = new Board();
        IDisplay display = new Display();
        GameController controller = new GameController(players, deck, board);

        controller.OnGameStart += () => display.DirectMessage("Welcome to the Domino Game!\n");
        controller.OnGameStart += () => display.DirectMessage("Generating Deck");
        controller.OnGameStart += controller.GenerateStandardDeck;
        controller.OnGameStart += () => display.LoadDot(3);
        controller.OnGameStart += () => display.DirectMessage("Shuffling");
        controller.OnGameStart += controller.Shuffle;
        controller.OnGameStart += () => display.LoadDot(3);
        controller.OnGameStart += controller.SetupPlayers;
        controller.OnGameStart += controller.DetermineFirstPlayer;
        controller.OnGameStart += () => display.AddMessage($"{controller.GetCurrentPlayer().Name} starts with [{controller.LeftEndValue}|{controller.RightEndValue}].\n");

        controller.OnPlayerTurn += controller.GetPlayableMoves;

        controller.OnGameOver += controller.CalculateScores;

        IPlayer PPlayer;
        ICard PCard;
        Side PSide;

        controller.StartGame();

        while (!controller.CheckGameOver())
        {
            display.ClearConsole();
            PPlayer = controller.GetCurrentPlayer();
            display.ShowMessage();
            display.ClearMessage();
            display.ShowBoard(board);
            display.ShowHand(PPlayer.Name, controller.GetHand(), controller.GetPlacableCards());

            if (controller.CanPlaceCard(controller.GetHand()))
            {
                while (true)
                {
                    PCard = display.PromptCard(controller.GetPlacableCards());
                    PSide = display.PromptSide();
                    if (controller.IsPlayable(PCard, PSide))
                    {
                        controller.ExecuteMove(PCard, PSide);
                        display.AddMessage($"{PPlayer.Name} placed [{PCard.LeftFaceValue}|{PCard.RightFaceValue}] on the {PSide} side.");
                        controller.NextTurn();
                        break;
                    }
                    else
                    {
                        display.AddMessage("Invalid move. Try again.");
                    }
                }
            }
            else
            {
                display.DirectMessage("No playable cards available. \n Passing turn");
                display.LoadDot(3);
                display.AddMessage($" {PPlayer.Name} passed their turn.");
                controller.PassTurn();
            }
            display.Wait();
        }
    }
}