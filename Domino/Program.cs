using Domino.Controllers;
using Domino.Interfaces;
using Domino.Models;
using Domino.Enumerations;
using System;

class Program
{
    private static void Main()
    {
        int maxPlayer = 4;
        int maxHandSize = 7;

        IDisplay screen = new Display();
        screen.Wait();

        ShowMainMenu(
            screen, ref maxPlayer, ref maxHandSize);

        List<IPlayer> players = CollectPlayers(screen, maxPlayer);

        IDeck deck = new Deck();
        IBoard board = new Board();
        GameController logic = new GameController(
            players, deck, board, maxHandSize);

        SetupGame(screen, logic);
        PlayGame(screen, logic);
        EndGame(screen, logic);
    }

    private static void ShowMainMenu(
        IDisplay screen, ref int maxPlayer, ref int maxHandSize)
    {
        bool start = false;
        while (!start)
        {
            screen.MainMenu();
            screen.DirectMessage(
                $"(Current settings: {maxPlayer} players with {maxHandSize} cards each)\n\nInsert an option: ");

            int menu = screen.PromptMenu();
            switch (menu)
            {
                case 1:
                    start = true;
                    break;

                case 2:
                    ConfigureGame(
                        screen, ref maxPlayer, ref maxHandSize);
                    break;

                case 3:
                    screen.Clear();
                    screen.DirectMessage(
                        "Thank you ^-^\n\nPress any key to exit...");

                    Environment.Exit(0); break;

                default:
                    screen.DirectMessage(
                        "Invalid option. Please try again");
                    break;
            }
        }
    }

    private static void ConfigureGame(
        IDisplay screen, ref int maxPlayer, ref int maxHandSize)
    {
        bool leave = false;
        while (!leave)
        {
            screen.ConfigMenu();
            int configOption = screen.PromptMenu();
            switch (configOption)
            {
                case 1:
                    screen.DirectMessage(
                        "Set number of players: ");
                    maxPlayer = screen.PromptMenu();
                    break;

                case 2:
                    screen.DirectMessage(
                        "Set number of cards per player: ");
                    maxHandSize = screen.PromptMenu();
                    break;

                case 3:
                    leave = true;
                    break;

                default:
                    screen.DirectMessage(
                        "Invalid option. Returning to main menu.\n");
                    break;
            }
        }
    }

    private static List<IPlayer> CollectPlayers(
        IDisplay screen, int maxPlayer)
    {
        List<IPlayer> players = new List<IPlayer>();
        for (int i = 0; i < maxPlayer; i++)
        {
            screen.Clear();
            screen.PrintHeader(
                "===== Player Join =====");
            screen.DirectMessage(
                $"\nEnter Player {i + 1} Name: ");

            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                screen.DirectMessage(
                    "Invalid name. Please try again.");

                i--; continue;
            }
            players.Add(new Player(name));
        }
        return players;
    }

    private static void SetupGame(
        IDisplay screen, GameController logic)
    {
        logic.OnGameStart += () => screen.DirectMessage("Generating Deck");
        logic.OnGameStart += logic.GenerateStandardDeck;
        logic.OnGameStart += () => screen.LoadDot(3);
        logic.OnGameStart += () => screen.DirectMessage("Shuffling");
        logic.OnGameStart += logic.Shuffle;
        logic.OnGameStart += () => screen.LoadDot(3);
        logic.OnGameStart += logic.SetupPlayers;
        logic.OnGameStart += logic.DetermineFirstPlayer;
        logic.OnGameStart += () => screen.AddMessage(
            $"= {logic.GetCurrentPlayer().Name} starts with [{logic.LeftEndValue}|{logic.RightEndValue}].");
        logic.OnGameStart += logic.NextTurn;

        logic.OnPlayerTurn += logic.GetPlayableMoves;
        logic.OnGameOver += logic.CalculateScores;

        logic.StartGame();
    }

    private static void PlayGame(
        IDisplay screen, GameController logic)
    {
        while (
            !logic.CheckGameOver())
        {
            IPlayer player = logic.GetCurrentPlayer();

            screen.Clear();

            screen.ShowGameInfo(
                logic.GetRound(), player.Name);
            screen.ShowMessage();
            screen.ShowBoard(
                logic.GetBoard());
            screen.ShowHand(
                player.Name,
                logic.GetHandCard(),
                logic.CreateCardPlacement());
            screen.ShowHint(
                logic.GetHandCard(),
                logic.CreateCardPlacement());

            if (logic.CanPlaceCard(logic.GetHandCard()))
            {
                while (true)
                {
                    ICard card = screen.PromptCard(logic.CreateCardPlacement());
                    Side side = screen.PromptSide();

                    if (logic.IsPlayable(card, side))
                    {
                        logic.ExecuteMove(card, side);
                        screen.AddMessage(
                            $"= {player.Name} placed [{card.LeftFaceValue}|{card.RightFaceValue}] on the {side} side.");

                        logic.NextTurn();
                        break;
                    }
                    else screen.AddMessage(
                        "Invalid move. Try again.");
                }
            }
            else
            {
                screen.DirectMessage(
                    "No playable cards available. \nPassing turn");
                screen.LoadDot(3);
                screen.AddMessage(
                    $"= {player.Name} passed their turn.");
                logic.PassTurn();
            }
            screen.Wait();
        }
    }

    private static void EndGame(
        IDisplay screen, GameController logic)
    {
        logic.OnGameOver?.Invoke();

        screen.Clear();
        screen.AddMessage(
            "Game Over!!!");
        screen.AddMessage(
            $"= {logic.GetWinner().Name} wins with the least point remaining {logic.GetWinner().Score}.");
        screen.ShowMessage();

        foreach (IPlayer player in logic.GetPlayers())
        {
            screen.DirectMessage(
                $"= {player.Name} has {player.Score} points of remaining cards.\n");
            screen.ShowHand(
                player.Name,
                logic.GetHandByPlayer(player),
                logic.CreateCardPlacement());
            screen.DirectMessage("\n");
        }

        screen.ShowScore(
            logic.GetScore());
        screen.DirectMessage(
            "\nThank you for playing!\nPress any key to exit...");

        Console.ReadKey();
    }
}