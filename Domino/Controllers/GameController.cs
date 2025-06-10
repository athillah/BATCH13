using Domino.Interfaces;
using Domino.Models;
using Domino.Enums;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace Domino.Controllers;

public class GameController
{
    private IDeck _deck;
    private IBoard _board;
    private List<IPlayer> _players;
    private Dictionary<IPlayer, List<ICard>> _hand;
    private List<ICard> _moveOptions = new();

    private IPlayer? _currentPlayer;
    private Dictionary<IPlayer, int>? _winScore;

    public Action? onGameStart;
    public Action<IPlayer>? onPlayerTurn;
    public Action? onGameOver;

    public int? LeftEndValue;
    public int? RightEndValue;

    private int _turnIndex;
    private int _round;
    private int _passCount;
    private int _maxHandSize;
    private int _maxDeckSize = 1;
    private Logger? _logger;

    public GameController(
        List<IPlayer> players, IDeck deck, IBoard board, int maxHandSize)
    {
        _players = players;
        _deck = deck;
        _board = board;
        _hand = new();
        _maxHandSize = maxHandSize;
        _logger = new("log.txt");

        AdjustDeckSize();
    }

    public void SetupPlayers()
    {
        _players.ForEach(
            SetHandCard);
    }

    public void StartGame()
    {
        _round = 0;
        _passCount = 0;
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
    {
        return _hand[_currentPlayer!];
    }

    public void DetermineFirstPlayer()
    {
        IPlayer? bestDoublePlayer = null;
        IPlayer? bestValuePlayer = null;
        ICard? bestDoubleCard = null;
        ICard? bestValueCard = null;

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

        SetFirstCard(firstcard!);
    }

    public void UpdateEndValues(int value, Side side)
    {
        if (side == Side.RIGHT)
            RightEndValue = value;
        else LeftEndValue = value;
    }

    public bool CanPlaceCard(List<ICard> hand)
    {
        return hand.Any(CanConnect);
    }

    public bool CanConnect(ICard card)
    {
        return card.GetValue().Any(
            v => v == LeftEndValue
              || v == RightEndValue);
    }

    public void FlipCard(ICard card)
    {
        (card.LeftFaceValue, card.RightFaceValue)
      = (card.RightFaceValue, card.LeftFaceValue);
    }

    public void GetPlayableMoves(IPlayer player)
    {
        _moveOptions.Clear();
        _moveOptions.AddRange(
            _hand[player]
            .Where(CanConnect));
    }

    public List<ICard> CreateCardPlacement()
    {
        return _moveOptions;
    }

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
        if ((side == Side.LEFT && card.LeftFaceValue == endValue)
        || (side == Side.RIGHT && card.RightFaceValue == endValue))
            FlipCard(card);

        if (side == Side.RIGHT)
            InsertRight(card);
        else InsertLeft(card);
    }

    public void NextTurn()
    {
        _round++;
        _turnIndex = (
            _turnIndex + 1) % _players.Count;
        _currentPlayer = _players[_turnIndex];
    }

    public void PassTurn()
    {
        _passCount++;
        NextTurn();
    }

    public void CalculateScores()
    {
        _winScore = new();

        foreach (var player in _players)
        {
            int value = GetHandValue(
                player);
            _winScore[player] = value;

            SetScore(player, value);
        }
    }

    public Dictionary<IPlayer, int> GetScores()
    {
        return _winScore
            ?? new();
    }

    public int GetRound()
    {
        return _round;
    }

    public void SetScore(IPlayer player, int score)
    {
        player.Score = score;
    }

    public List<IPlayer> GetPlayers()
    {
        return _players;
    }

    public int GetScore(IPlayer player)
    {
        return player.Score;
    }

    public IPlayer GetWinner()
    {
        return _players.OrderBy(
            GetScore).First();
    }

    public bool CheckGameOver()
    {
        return IsFullPass()
            || IsAnyHandEmpty();
    }

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
        var card =
        _deck.Cards[0];
        _deck.Cards.RemoveAt(0);

        return card;
    }

    private void AdjustDeckSize()
    {
        while ((_players.Count * _maxHandSize)
                          > 28 * _maxDeckSize)
            _maxDeckSize++;
    }

    private void SetFirstCard(ICard card)
    {
        _board.PlayedCards.Add(card);

        UpdateEndValues(
            card.LeftFaceValue, Side.LEFT);
        UpdateEndValues(
            card.RightFaceValue, Side.RIGHT);

        _hand[_currentPlayer!]
            .Remove(card);
    }

    public IPlayer GetCurrentPlayer()
    {
        return _currentPlayer
            ?? throw new InvalidOperationException("No current player set.");
    }

    public IBoard GetBoard()
    {
        if (_board.PlayedCards.Count == 12)
            RefreshBoard();

        return _board;
    }

    private void RefreshBoard()
    {
        var board = _board.PlayedCards;
        var deck = _deck.Cards;

        var first = board.First();
        var middle = board.Skip(1).Take(board.Count - 2).ToList();
        var last = board.Last();

        board.Clear();
        board.Add(first);
        board.Add(last);

        deck.AddRange(middle);
        Shuffle();
    }

    public int GetHandValue(IPlayer player)
    {
        return _hand[player].Sum(
            c => c.IsDouble() ? (
                c.LeftFaceValue == 0 ? 20 : c.LeftFaceValue * 2) :
                c.LeftFaceValue + c.RightFaceValue);
    }

    public List<ICard> GetHandByPlayer(IPlayer player)
    {
        return _hand[player];
    }

    public bool IsPlayable(ICard card, Side side)
    {
        int? end = side == Side.LEFT
           ? LeftEndValue
           : RightEndValue;

        return card.LeftFaceValue == end
            || card.RightFaceValue == end;
    }

    private void InsertLeft(ICard card)
    {
        _board.PlayedCards.Insert(0, card);

        UpdateEndValues(
            card.LeftFaceValue,
            Side.LEFT);
    }

    private void InsertRight(ICard card)
    {
        _board.PlayedCards.Add(card);

        UpdateEndValues(
            card.RightFaceValue,
            Side.RIGHT);
    }

    private bool IsFullPass()
    {
        return _passCount
            >= _players.Count;
    }

    private bool IsAnyHandEmpty()
    {
        return _players.Any(
            p => _hand[p].
            Count == 0);
    }

    private class Logger
    {
        private string _filepath;
        public Logger(string filepath)
        {
            _filepath = filepath;
            using (var fs = new FileStream(
                _filepath, FileMode.Create, FileAccess.Write)) { }
        }
        public void Log(string log)
        {
            using (var fs = new FileStream(_filepath, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
                writer.WriteLine(log);
        }
    }

    public void Clear()
    {
        _deck.Cards.Clear();
        _board.PlayedCards.Clear();
        _players.Clear();
        _hand.Clear();
        _moveOptions.Clear();
        _winScore?.Clear();
    }
}
