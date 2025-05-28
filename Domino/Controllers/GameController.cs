using Domino.Interfaces;
using Domino.Models;
using Domino.Enumerations;
using System;
using System.Text;
using System.Security.Cryptography;

namespace Domino.Controllers;

public class GameController
{
    private IDeck _deck;
    private IBoard _board;
    private List<IPlayer> _players;
    private Dictionary<IPlayer, List<ICard>> _hand;
    private Dictionary<int, ICard> _placableCards;
    private IPlayer _currentPlayer;
    private int _turnIndex;
    private int _round;
    private int _playerCount;
    private int _passCount = 0;
    //private int _maxPlayers;
    private int[] _winScore;
    StringBuilder _message;

    private Action OnGameStart;
    private Action<IPlayer> OnPlayerTurn;
    private Action<bool> OnGameOver;

    public Side PlacementSide;
    //public bool IsFlipped;
    //public int ConnectingValue;
    public bool CanPlace;
    //public Side AllowedSide;
    //public bool NeedsFlip;
    public int? LeftEndValue = null;
    public int? RightEndValue = null;

    private ConsoleColor _originalBackground;
    private ConsoleColor _originalForeground;

    public GameController(List<IPlayer> players, IDeck deck, IBoard board)
    {
        Console.Clear();
        _players = players;
        _playerCount = players.Count;
        _deck = deck;
        _board = board;
        _hand = new Dictionary<IPlayer, List<ICard>>();
        _turnIndex = 0;
        _placableCards = new Dictionary<int, ICard>();
        OnGameStart += GenerateStandardDeck;
        OnGameStart += Shuffle;
        OnGameStart += SetupPlayers;
        OnGameStart += DetermineFirstPlayer;
        OnPlayerTurn += GetPlayableMoves;
        OnPlayerTurn += ShowBoard;
        OnPlayerTurn += ShowHand;
        _originalBackground = Console.BackgroundColor;
        _originalForeground = Console.ForegroundColor;
        _message = new StringBuilder();
        Console.WriteLine("Welcome to Dominomatrix!\n");
        Thread.Sleep(1000);
    }
    public void SetupPlayers()
    {
        AssignPlayerNames();
        foreach (IPlayer player in _players)
        {
            SetHandCard(player);
        }
    }
    public void AssignPlayerNames()
    {
        foreach (IPlayer player in _players)
        {
            player.Name = "Player " + (_players.IndexOf(player) + 1);
        }
    }
    public void StartGame()
    {
        Console.WriteLine("Intializing game...");
        Thread.Sleep(1000);
        OnGameStart();
        _round = 1;
        while (!CheckGameOver())
        {
            Console.Clear();
            Console.WriteLine(_message);
            _message.Clear();
            Thread.Sleep(500);
            OnPlayerTurn(_currentPlayer);
            if (CanPlaceCard(_hand[_currentPlayer]))
            {
                _passCount = 0;
                Console.Write($"\nPlay a card (input eg 50 for [5|0]): ");
                int cardId = int.Parse(Console.ReadLine() ?? "0");
                ICard cardToPlace = _hand[_currentPlayer].FirstOrDefault(c => c.Id == cardId);
                Console.Write("Choose side to place (LEFT/RIGHT): ");
                PlacementSide = (Side)Enum.Parse(typeof(Side), Console.ReadLine()?.ToUpper() ?? "LEFT");
                if (cardToPlace != null && CanConnect(cardToPlace))
                {
                    PlaceCardOnBoard(cardToPlace, PlacementSide);
                    _hand[_currentPlayer].Remove(cardToPlace);
                    ShowBoard(_currentPlayer);
                    ShowHand(_currentPlayer);
                }
                else
                {
                    Console.WriteLine("Invalid card or cannot connect to the board.");
                }
            }
            else
            {
                PassTurn();
            }
            TurnOrder();
        }
    }
    public void SetHandCard(IPlayer player)
    {
        _hand[player] = new List<ICard>();
        for (int i = 0; i < 7; i++)
        {
            if (_deck.Cards.Count > 0)
            {
                _hand[player].Add(DrawCard());
            }
            else
            {
                Console.WriteLine("Deck is empty, cannot draw more cards.");
                break;
            }
        }
    }
    // public ICard GetHandCard(IPlayer player)
    // {
    //     return _hand[player][0];
    // }
    public void TurnOrder()
    {
        _round++;
        _turnIndex = (_turnIndex + 1) % _players.Count;
        _currentPlayer = _players[_turnIndex];
        _message.Append($"\nRound {_round}: Player {_currentPlayer.Name}");
    }
    public void DetermineFirstPlayer()
    {
        Console.Write("\nDetermining first player");
        IPlayer highestDoublePlayer = null;
        IPlayer highestValuePlayer = null;
        ICard highestDoubleCard = null;
        ICard highestValueCard = null;
        foreach (IPlayer player in _players)
        {
            foreach (ICard card in _hand[player])
            {
                if (card.IsDouble())
                {
                    if (highestDoubleCard == null || card.LeftFaceValue > highestDoubleCard.LeftFaceValue)
                    {
                        highestDoubleCard = card;
                        highestDoublePlayer = player;
                    }
                }
                if (highestValueCard == null || card.LeftFaceValue + card.RightFaceValue > highestValueCard.LeftFaceValue + highestValueCard.RightFaceValue)
                {
                    highestValueCard = card;
                    highestValuePlayer = player;
                }
            }
            Console.Write("..");
            Thread.Sleep(500);
        }
        _currentPlayer = highestDoublePlayer ?? highestValuePlayer;
        ICard firstcard = highestDoubleCard ?? highestValueCard;

        _message.Append($"\nFirst card played by {_currentPlayer.Name} with {firstcard.LeftFaceValue}|{firstcard.RightFaceValue}");

        _board.PlayedCards.Add(firstcard);
        LeftEndValue = firstcard.LeftFaceValue;
        RightEndValue = firstcard.RightFaceValue;
        _hand[_currentPlayer].Remove(firstcard);
        TurnOrder();
        _round = 0;
    }

    public int? GetLeftEndValue()
    {
        return LeftEndValue;
    }
    public int? GetRightEndValue()
    {
        return RightEndValue;
    }
    public void UpdateEndValues(int value, Side side)
    {
        if (side == Side.LEFT)
        {
            LeftEndValue = value;
        }
        else if (side == Side.RIGHT)
        {
            RightEndValue = value;
        }
    }
    public bool CanPlaceCard(List<ICard> hand) //nyari posisi kanan kiri = ngubah placement side
    {
        foreach (ICard card in hand)
        {
            if (CanConnect(card))
            {
                return true;
            }
        }
        return false;
    }
    public bool CanConnect(ICard card) // nyari value dari sisi card
    {
        foreach (int value in card.GetValue())
        {
            if (value == LeftEndValue || value == RightEndValue)
            {
                return true;
            }
        }
        return false;
    }
    public int? GetConnectingValue(Side side)
    {
        return side == Side.LEFT ? LeftEndValue : RightEndValue;
    }
    //public PlacementInfo CreatePlacementInfo(bool canPlace, Side side, bool needsFlip)
    public void FlipCard(ICard card)
    {
        int temp = card.RightFaceValue;
        card.RightFaceValue = card.LeftFaceValue;
        card.LeftFaceValue = temp;
    }
    public void GetPlayableMoves(IPlayer player)
    {
        _placableCards.Clear();
        int index = 0;
        foreach (ICard card in _hand[player])
        {
            if (CanConnect(card))
            {
                _placableCards[index] = card;
                index++;
            }
        }
    }
    //public CardPlacement CreateCardPlacement(ICard card, Side side, bool flipped, int value)
    //public bool ExecuteMove(int option, Side placementSide)
    public void PlaceCardOnBoard(ICard card, Side side)
    {
        _message.Append($"\n{_currentPlayer.Name} played {card.LeftFaceValue}|{card.RightFaceValue}.");
        if (side == Side.LEFT)
        {
            if (card.LeftFaceValue == LeftEndValue)
            {
                FlipCard(card);
            }
            _board.PlayedCards.Insert(0, card);
            UpdateEndValues(card.LeftFaceValue, Side.LEFT);
        }
        if (side == Side.RIGHT)
        {
            if (card.RightFaceValue == RightEndValue)
            {
                FlipCard(card);
            }
            _board.PlayedCards.Insert(_board.PlayedCards.Count, card);
            UpdateEndValues(card.RightFaceValue, Side.RIGHT);
        }
    }
    //public NextTurn(Action<IPlayer> OnPlayerTurn)
    public void PassTurn()
    {
        _message.Append($"\n{_currentPlayer.Name} has passed their turn.");
        _passCount++;
    }
    public void ShowBoard(IPlayer player)
    {
        Console.BackgroundColor = ConsoleColor.Green;
        foreach (ICard card in _board.PlayedCards)
        {
            Console.Write(" ");
            SetDominoColor();
            Console.Write($"{card.LeftFaceValue}|{card.RightFaceValue}");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" ");
        }
        Console.WriteLine($"\nLEFT {LeftEndValue} RIGHT {RightEndValue}\n");
        ResetConsoleColor();
    }
    public void ShowHand(IPlayer player)
    {
        Console.WriteLine($"{player.Name}'s hand: ");
        foreach (ICard card in _hand[player])
        {
            if (CanConnect(card))
            {
                SetDominoColor();
                Console.Write($" {card.LeftFaceValue}|{card.RightFaceValue} ");
                ResetConsoleColor();
                Console.Write(" ");
            }
            else
            {
                SetReversedDominoColor();
                Console.Write($" {card.LeftFaceValue}|{card.RightFaceValue} ");
                ResetConsoleColor();
                Console.Write(" ");
            }
        }
    }
    //public int CalculateScore(IPlayer player)
    public void SetScore(IPlayer player, int score)
    {
        player.Score = score;
    }
    public int GetScore(IPlayer player)
    {
        return player.Score;
    }
    //public IPlayerGetWinner()
    public bool CheckGameOver() {
        if (_passCount >= _playerCount)
        {
            return true;
        }
        return false;
    }
    public ICard CreateCard(int id, int right, int left)
    {
        return new Card(id, right, left);
    }
    public void GenerateStandardDeck()
    {
        Console.Write("Creating deck");
        for (int i = 0; i <= 6; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                _deck.Cards.Add(CreateCard((i * 10) + j, j, i));
            }
            Thread.Sleep(500);
            Console.Write(".");
        }
    }
    public void Shuffle()
    {
        Console.Write("\nShuffling deck");
        Random random = new Random();
        List<ICard> cards = _deck.Cards;
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            ICard temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
            if (i % 4 == 0)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
        }
    }
    public ICard DrawCard()
    {
        ICard drawnCard = _deck.Cards[0];
        _deck.Cards.RemoveAt(0);
        return drawnCard;
    }

    public void SetDominoColor()
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Red;
    }
    public void SetReversedDominoColor()
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.Yellow;
    }
    public void ResetConsoleColor()
    {
        Console.BackgroundColor = _originalBackground;
        Console.ForegroundColor = _originalForeground;
    }
}
