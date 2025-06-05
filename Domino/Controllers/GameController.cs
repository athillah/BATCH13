using Domino.Interfaces;
using Domino.Models;
using Domino.Enumerations;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Domino.Controllers;

public class GameController
{
    // ReadOnlies ++ Privates
    private readonly IDeck _deck;
    private readonly IBoard _board;
    private readonly List<IPlayer> _players;
    private readonly Dictionary<IPlayer, List<ICard>> _hand;

    private IPlayer? _currentPlayer;
    private Dictionary<IPlayer, int>? _winScore;

    // Actions ++ Publics
    public Action? OnGameStart;
    public Action<IPlayer>? OnPlayerTurn;
    public Action? OnGameOver;

    public int? LeftEndValue, RightEndValue;


    // Unused Diagram's Property
    // private Side _placementSide;
    // public bool IsFlipped;
    // public int ConnectingValue;
    // public bool CanPlace;
    // public Side AllowedSide;
    // public bool NeedsFlip;
    // private int _playerCount;
    // private int _maxPlayers;
    // private Dictionary<int, ICard> moveOptions

    // Outside of Class Diagram
    private readonly List<ICard> _placableCards = new();
    private int _turnIndex, _round, _passCount;
    private int _maxHandSize { get; set; }
    private int _maxDeckSize = 1;
    private Logger? _logger;

    public GameController(
        List<IPlayer> players,
        IDeck deck,
        IBoard board,
        int maxHandSize)
    {
        _players = players;
        _deck = deck;
        _board = board;
        _hand = new();
        _maxHandSize = maxHandSize;

        _logger = new("log.txt");
        adjustDeckSize();
    }

    public void SetupPlayers()
    {
        AssignPlayerNames();
        _players.ForEach(
            SetHandCard);
    }

    public void AssignPlayerNames()
        => _players.ForEach(
            p => p.Name = p.Name.Trim());

    public void StartGame()
    {
        _round = _passCount = 0;
        OnGameStart?.Invoke();
    }

    public void SetHandCard(IPlayer player)
    {
        var hand = new List<ICard>();
        for (int i = 0; i < _maxHandSize && !_deck.IsEmpty(); i++)
            hand.Add(
                DrawCard());

        _hand[player] = hand;
    }

    public List<ICard> GetHandCard()
        => _currentPlayer != null
        && _hand.ContainsKey(_currentPlayer)
        ? _hand[_currentPlayer]
        : new List<ICard>();

    public void DetermineFirstPlayer()
    {
        IPlayer? bestDoublePlayer = null,
                  bestValuePlayer = null;
        ICard? bestDoubleCard = null,
                bestValueCard = null;

        foreach (var player in _players)
            foreach (var card in _hand[player])
            {
                if (card.IsDouble())
                    if (bestDoubleCard == null
                     || card.LeftFaceValue
                      > bestDoubleCard.LeftFaceValue)
                    {
                        bestDoubleCard = card;
                        bestDoublePlayer = player;
                    }

                if (bestValueCard == null
                 || card.LeftFaceValue + card.RightFaceValue
                  > bestValueCard.LeftFaceValue + bestValueCard.RightFaceValue)
                {
                    bestValueCard = card;
                    bestValuePlayer = player;
                }
            }

        _currentPlayer = bestDoublePlayer
                      ?? bestValuePlayer;

        _turnIndex = _players.IndexOf(
            _currentPlayer!);

        var firstcard = bestDoubleCard
                     ?? bestValueCard;

        if (firstcard != null)
            setFirstCard(firstcard);
    }

    public void UpdateEndValues(int value, Side side)
    {
        if (side == Side.LEFT)
            LeftEndValue = value;
        else RightEndValue = value;
    }

    public bool CanPlaceCard(List<ICard> hand)
        => hand.Any(
            CanConnect);

    public bool CanConnect(ICard card)
        => card.GetValue().Any(
            v => v == LeftEndValue
              || v == RightEndValue);

    public void FlipCard(ICard card)
        => (card.LeftFaceValue, card.RightFaceValue)
         = (card.RightFaceValue, card.LeftFaceValue);

    public void GetPlayableMoves(IPlayer player)
    {
        _placableCards.Clear();
        _placableCards.AddRange(
            _hand[player]
            .Where(CanConnect));
    }

    public List<ICard> CreateCardPlacement()
        => _placableCards;

    public void ExecuteMove(ICard card, Side side)
    {
        _passCount = 0;
        PlaceCardOnBoard(card, side);
        _hand[_currentPlayer!]
            .Remove(card);
    }

    public void PlaceCardOnBoard(ICard card, Side side)
    {
        int? endValue = side == Side.LEFT ? LeftEndValue : RightEndValue;
        if ((side == Side.LEFT &&
             card.LeftFaceValue == endValue)
        || (side == Side.RIGHT &&
             card.RightFaceValue == endValue))
            FlipCard(card);

        if (side == Side.LEFT)
            insertLeft(card);
        else insertRight(card);
    }

    public void NextTurn()
    {
        _round++;

        _turnIndex = (_turnIndex + 1)
                   % _players.Count;
        _currentPlayer = _players
            [_turnIndex];
        OnPlayerTurn?.Invoke(_currentPlayer);
    }

    public void PassTurn()
    {
        _passCount++; NextTurn();
    }

    public void CalculateScores()
    {
        _winScore = new();
        foreach (var player in _players)
        {
            int value = GetHandValue(
                player);
            player.Score = value;

            _winScore[player] = value;
        }
    }

    public Dictionary<IPlayer, int> GetScore()
        => _winScore ?? new();

    public int GetRound()
        => _round;

    public List<IPlayer> GetPlayers()
        => _players;

    public int GetScore(IPlayer player)
        => player.Score;

    public IPlayer GetWinner()
        => _players.OrderBy(
            GetScore).First();

    public bool CheckGameOver()
        => isFullPass()
        || isAnyHandEmpty();

    public void GenerateStandardDeck()
    {
        for (int d = 0; d < _maxDeckSize; d++)
            for (int l = 0; l <= 6; l++)
                for (int r = 0; r <= l; r++)
                    _deck.Cards.Add(
                        new Card((l * 10) + r, l, r));
    }

    public void Shuffle()
    {
        var cards = _deck.Cards;
        var random = new Random();

        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (cards[i], cards[j]) =
            (cards[j], cards[i]);
        }
    }

    public ICard DrawCard()
    {
        var card = _deck.Cards[0];
        _deck.Cards.RemoveAt(0);

        return card;
    }

    // Unused Diagram's Method
    // private ICard createCard(int id, int left, int right) => new Card(id, left, right);
    // public void SetScore(IPlayer player, int score) => player.Score = score;
    // public ShowBoard()
    // public void ShowHand()
    // public int? GetConnectingValue(Side side) => return side == Side.LEFT ? LeftEndValue : RightEndValue;
    // public PlacementInfo CreatePlacementInfo(bool canPlace, Side side, bool needsFlip)
    // public int? GetLeftEndValue() => LeftEndValue;
    // public int? GetRightEndValue() => RightEndValue;
    // public void TurnOrder() => _currentPlayer = _players[0];

    // Outside of Class Diagram
    private void adjustDeckSize()
    {
        _logger?.Log(
            $"in adjust\n{_players.Count}*{_maxHandSize} >< {28 * _maxDeckSize}");
        while ((_players.Count * _maxHandSize)
                          > 28 * _maxDeckSize)
            _maxDeckSize++;
    }

    private void setFirstCard(ICard card)
    {
        _board.PlayedCards.Add(card);

        LeftEndValue = card
             .LeftFaceValue;
        RightEndValue = card
             .RightFaceValue;

        _hand[_currentPlayer!]
            .Remove(card);
    }
    public IPlayer GetCurrentPlayer()
        => _currentPlayer
        ?? throw new InvalidOperationException("No current player set.");

    public IBoard GetBoard()
        => _board;

    public int GetHandValue(IPlayer player)
        => _hand[player].Sum
            (c => c.IsDouble()
        ? (c.LeftFaceValue == 0 ? 20 : c.LeftFaceValue * 2)
        : c.LeftFaceValue + c.RightFaceValue);

    public List<ICard> GetHandByPlayer(IPlayer player)
        => _hand[player];

    public bool IsPlayable(ICard card, Side side)
    {
        int? end = side == Side.LEFT
           ? LeftEndValue
           : RightEndValue;

        return card.LeftFaceValue == end
            || card.RightFaceValue == end;
    }

    private void insertLeft(ICard card)
    {
        _board.PlayedCards.Insert(0, card);

        UpdateEndValues(
            card.LeftFaceValue,
            Side.LEFT);
    }

    private void insertRight(ICard card)
    {
        _board.PlayedCards.Add(card);

        UpdateEndValues(
            card.RightFaceValue,
            Side.RIGHT);
    }

    private bool isFullPass()
        => _passCount
        >= _players.Count;

    private bool isAnyHandEmpty()
        => _players.Any(
            p => _hand[p].
            Count == 0);


    private class Logger
    {
        private readonly string _filepath;
        public Logger(string filepath)
        {
            _filepath = filepath;
            using (var fs = new FileStream(
                _filepath, FileMode.Create, FileAccess.Write))
            { }
        }
        public void Log(string log)
        {
            using (
                var fs = new FileStream(
                    _filepath, FileMode.Append, FileAccess.Write))
            using (
                var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(log);
            }
        }
    }
}