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
        IDisplay display = new Display();
        bool start = false;
        int maxPlayer = 4;
        int maxHandSize = 7;

        display.ClearConsole();
        display.Wait();
        while (!start)
        {
            display.MainMenu();
            display.DirectMessage($"(Current settings: {maxPlayer} players with {maxHandSize} cards each)\n\n");
            Console.Write("Insert an option: ");
            int menu = display.PromptMenu();
            switch (menu)
            {
                case 1:
                    start = true;
                    break;
                case 2:
                    display.ConfigMenu();
                    menu = display.PromptMenu();
                    switch (menu)
                    {
                        case 1:
                            display.DirectMessage("Set number of players: ");
                            maxPlayer = display.PromptMenu();
                            break;
                        case 2:
                            display.DirectMessage("Set number of cards per player: ");
                            maxHandSize = display.PromptMenu();
                            break;
                        case 3:
                            break;
                        default:
                            display.DirectMessage("Invalid option. Returning to main menu.\n");
                            break;
                    }
                    break;
                case 3:
                    display.ClearConsole();
                    display.DirectMessage("Press any key to exit...");
                    Environment.Exit(0);
                    break;
                default:
                    display.DirectMessage("Invalid option. Please try again");
                    break;
            }
        }

        List < IPlayer > players = new List<IPlayer>();
        for (int playerJoin = 0; playerJoin < maxPlayer; playerJoin++)
        {
            display.ClearConsole();
            display.DirectMessage("===== Player Join =====\n");
            display.DirectMessage($"\nEnter Player {playerJoin + 1} Name: ");
            string? playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                display.DirectMessage("Invalid name. Please try again.");
                playerJoin--;
                continue;
            }
            IPlayer player = new Player(playerName);
            players.Add(player);
        }

        IDeck deck = new Deck();
        IBoard board = new Board();
        GameController controller = new GameController(players, deck, board, maxHandSize);

        controller.OnGameStart += () => display.DirectMessage("Generating Deck");
        controller.OnGameStart += controller.GenerateStandardDeck;
        controller.OnGameStart += () => display.LoadDot(3);
        controller.OnGameStart += () => display.DirectMessage("Shuffling");
        controller.OnGameStart += controller.Shuffle;
        controller.OnGameStart += () => display.LoadDot(3);
        controller.OnGameStart += controller.SetupPlayers;
        controller.OnGameStart += controller.DetermineFirstPlayer;
        controller.OnGameStart += () => display.AddMessage($"= {controller.GetCurrentPlayer().Name} starts with [{controller.LeftEndValue}|{controller.RightEndValue}].\n");
        controller.OnGameStart += controller.NextTurn;

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
            display.ShowGameInfo(controller.GetRound(), PPlayer.Name);
            display.ShowMessage();
            display.ClearMessage();
            display.ShowBoard(controller.GetBoard());
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
                        display.AddMessage($"= {PPlayer.Name} placed [{PCard.LeftFaceValue}|{PCard.RightFaceValue}] on the {PSide} side.");
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
                display.DirectMessage("No playable cards available. \nPassing turn");
                display.LoadDot(3);
                display.AddMessage($"= {PPlayer.Name} passed their turn.");
                controller.PassTurn();
            }
            display.Wait();
        }
        controller.OnGameOver?.Invoke();

        display.ClearConsole();
        display.AddMessage("\nGame Over!");
        display.AddMessage($"= {controller.GetWinner().Name} wins with the least point remaining {controller.GetWinner().Score}");
        display.ShowMessage();
        foreach (IPlayer player in controller.GetPlayers())
        {
            display.DirectMessage($"= {player.Name} has {player.Score} points of remaining cards");
            display.ShowHand(player.Name, controller.GetHandByPlayer(player), controller.GetPlacableCards());
        }
        display.ShowScore(controller.GetScores());
        display.DirectMessage("\nThank you for playing!\nPress any key to exit...");
        Console.ReadKey();
        display.ClearConsole();
    }
}