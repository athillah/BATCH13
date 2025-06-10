using Domino.Controllers;
using Domino.Interfaces;
using Domino.Models;
using Domino.Enums;
using System;

class Program
{
    private static void Main()
    {
        int maxPlayer = 4;
        int maxHandSize = 7;

        IDisplay screen = new Display();
        screen.Wait();

        ShowMainMenu(screen, ref maxPlayer, ref maxHandSize);

        List<IPlayer> players = CollectPlayers(screen, maxPlayer);

        IDeck deck = new Deck();
        IBoard board = new Board();
        GameController logic = new GameController(players, deck, board, maxHandSize);

        SetupGame(screen, logic);
        PlayGame(screen, logic);
        EndGame(screen, logic);
    }

    private static void ShowMainMenu(IDisplay screen, ref int maxPlayer, ref int maxHandSize)
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
                    Environment.Exit(0); ;
                    break;

                default:
                    screen.DirectMessage(
                        "Invalid option. Please try again");
                    break;
            }
        }
    }

    private static void ConfigureGame(IDisplay screen, ref int maxPlayer, ref int maxHandSize)
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

    private static List<IPlayer> CollectPlayers(IDisplay screen, int maxPlayer)
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
    private static void Unsubscribe(IDisplay screen, GameController logic)
    {
        logic.onGameStart -= () => screen.DirectMessage("Generating Deck");
        logic.onGameStart -= logic.GenerateStandardDeck;
        logic.onGameStart -= () => screen.LoadDot(3);
        logic.onGameStart -= () => screen.DirectMessage("Shuffling");
        logic.onGameStart -= logic.Shuffle;
        logic.onGameStart -= () => screen.LoadDot(3);
        logic.onGameStart -= logic.SetupPlayers;
        logic.onGameStart -= logic.DetermineFirstPlayer;
        logic.onGameStart -= () => screen.AddMessage(
            $"█ {logic.GetCurrentPlayer().Name} starts with [{logic.LeftEndValue}|{logic.RightEndValue}].");
        logic.onGameStart -= () => logic.StartGame();
        logic.onGameStart -= logic.NextTurn;

        logic.onPlayerTurn -= logic.GetPlayableMoves;
        logic.onPlayerTurn -= (player) => screen.Clear();
        logic.onPlayerTurn -= (player) => screen.ShowGameInfo(
            logic.GetRound(), player.Name);
        logic.onPlayerTurn -= (player) => screen.ShowMessage();
        logic.onPlayerTurn -= (player) => screen.ShowBoard(logic.GetBoard());
        logic.onPlayerTurn -= (player) => screen.ShowHand(
            player.Name, logic.GetHandCard(), logic.CreateCardPlacement());
        logic.onPlayerTurn -= (player) => screen.ShowHint(
                logic.GetHandCard(), logic.CreateCardPlacement());

        logic.onGameOver -= logic.CalculateScores;
        logic.onGameOver -= () => screen.Clear();
        logic.onGameOver -= () => screen.AddMessage("█ Game Over!!!");
        logic.onGameOver -= () => screen.AddMessage(
            $"█ {logic.GetWinner().Name} wins with the least point remaining {logic.GetWinner().Score}.");
        logic.onGameOver -= () => screen.ShowMessage();
        logic.onGameOver -= () => logic.Clear();
    }

    private static void Subscribe(IDisplay screen, GameController logic)
    {
        logic.onGameStart += () => screen.DirectMessage("Generating Deck");
        logic.onGameStart += logic.GenerateStandardDeck;
        logic.onGameStart += () => screen.LoadDot(3);
        logic.onGameStart += () => screen.DirectMessage("Shuffling");
        logic.onGameStart += logic.Shuffle;
        logic.onGameStart += () => screen.LoadDot(3);
        logic.onGameStart += logic.SetupPlayers;
        logic.onGameStart += logic.DetermineFirstPlayer;
        logic.onGameStart += () => screen.AddMessage(
            $"█ {logic.GetCurrentPlayer().Name} starts with [{logic.LeftEndValue}|{logic.RightEndValue}].");
        logic.onGameStart += () => logic.StartGame();
        logic.onGameStart += logic.NextTurn;

        logic.onPlayerTurn += logic.GetPlayableMoves;
        logic.onPlayerTurn += (player) => screen.Clear();
        logic.onPlayerTurn += (player) => screen.ShowGameInfo(
            logic.GetRound(), player.Name);
        logic.onPlayerTurn += (player) => screen.ShowMessage();
        logic.onPlayerTurn += (player) => screen.ShowBoard(logic.GetBoard());
        logic.onPlayerTurn += (player) => screen.ShowHand(
            player.Name, logic.GetHandCard(), logic.CreateCardPlacement());
        logic.onPlayerTurn += (player) => screen.ShowHint(
                logic.GetHandCard(), logic.CreateCardPlacement());

        logic.onGameOver += logic.CalculateScores;
        logic.onGameOver += () => screen.Clear();
        logic.onGameOver += () => screen.AddMessage("█ Game Over!!!");
        logic.onGameOver += () => screen.AddMessage(
            $"█ {logic.GetWinner().Name} wins with the least point remaining {logic.GetWinner().Score}.");
        logic.onGameOver += () => screen.ShowMessage();
        logic.onGameOver += () => logic.Clear();
    }

    private static void SetupGame(IDisplay screen, GameController logic)
    {
        Subscribe(screen, logic);
        logic.onGameStart?.Invoke();
    }

    private static void PlayGame(IDisplay screen, GameController logic)
    {
        while (!logic.CheckGameOver())
        {
            IPlayer player = logic.GetCurrentPlayer();
            logic.onPlayerTurn?.Invoke(logic.GetCurrentPlayer());

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
                            $"█ {player.Name} placed [{card.LeftFaceValue}|{card.RightFaceValue}] on the {side} side.");

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
                    $"█ {player.Name} passed their turn.");
                logic.PassTurn();
            }
            screen.Wait();
        }
    }

    private static void EndGame(IDisplay screen, GameController logic)
    {
        logic.onGameOver?.Invoke();

        foreach (IPlayer player in logic.GetPlayers())
        {
            screen.DirectMessage(
                $"█ {player.Name} has {player.Score} points of remaining cards.");
            screen.ShowHand(
                player.Name,
                logic.GetHandByPlayer(player),
                logic.CreateCardPlacement());
            screen.DirectMessage("\n");
        }

        screen.ShowScore(
            logic.GetScores());
        screen.DirectMessage(
            "\nThank you for playing!\n");

        Unsubscribe(screen, logic);
    }
}