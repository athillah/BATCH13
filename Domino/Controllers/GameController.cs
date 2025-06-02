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
    private List<ICard> _placableCards;
    private IPlayer? _currentPlayer;
    private int _turnIndex;
    private int _round;
    private int _playerCount;
    private int _passCount = 0;
    //private int _maxPlayers;
    private int[]? _winScore;

    public Action OnGameStart;
    public Action<IPlayer> OnPlayerTurn;
    public Action OnGameOver;

    private Side _placementSide;
    //public bool IsFlipped;
    //public int ConnectingValue;
    //public bool CanPlace;
    //public Side AllowedSide;
    //public bool NeedsFlip;
    public int? LeftEndValue = null;
    public int? RightEndValue = null;

    public GameController(List<IPlayer> players, IDeck deck, IBoard board)
    {
        _players = players;
        _playerCount = players.Count;
        _deck = deck;
        _board = board;
        _hand = new Dictionary<IPlayer, List<ICard>>();
        _placableCards = new List<ICard>();
    }
    public void SetupPlayers()
    {
        AssignPlayerNames();
        foreach (IPlayer player in _players) SetHandCard(player);
    }
    public void AssignPlayerNames()
        => _players.ForEach(player => player.Name = player.Name.Trim());

    public void StartGame()
    {
        _round = 0;
        _turnIndex = 0;
        _passCount = 0;
        OnGameStart?.Invoke();
    }
    public void SetHandCard(IPlayer player)
    {
        _hand[player] = new List<ICard>();
        for (int i = 0; i < 7; i++) if (!_deck.IsEmpty()) _hand[player].Add(DrawCard());
    }
    // public ICard GetHandCard(IPlayer player)
    // {
    //     return _hand[player][0];
    // }
    public void TurnOrder()
        => _currentPlayer = _players[0];
    public void DetermineFirstPlayer()
    {

        IPlayer? highestDoublePlayer = null;
        IPlayer? highestValuePlayer = null;
        ICard? highestDoubleCard = null;
        ICard? highestValueCard = null;
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
        }
        _currentPlayer = highestDoublePlayer ?? highestValuePlayer;
        ICard? firstcard = highestDoubleCard ?? highestValueCard;

        setFirstCard(firstcard);
    }

    private void setFirstCard(ICard card)
    {
        _board.PlayedCards.Add(card);
        LeftEndValue = card.LeftFaceValue;
        RightEndValue = card.RightFaceValue;
        _hand[_currentPlayer].Remove(card);
    }
    public int? GetLeftEndValue()
        => LeftEndValue;
    public int? GetRightEndValue()
        => RightEndValue;
    public void UpdateEndValues(int value, Side side)
    {
        switch (side)
        {
            case Side.LEFT:
                LeftEndValue = value;
                break;
            case Side.RIGHT:
                RightEndValue = value;
                break;
        }
    }
public bool CanPlaceCard(List<ICard> hand)
    => hand.Any(CanConnect);

    public bool CanConnect(ICard card)
        => card.GetValue().Any(value => value == LeftEndValue || value == RightEndValue);

    // public int? GetConnectingValue(Side side)
    // {
    //     return side == Side.LEFT ? LeftEndValue : RightEndValue;
    // }
    //public PlacementInfo CreatePlacementInfo(bool canPlace, Side side, bool needsFlip)
    public void FlipCard(ICard card)
    {
        (card.LeftFaceValue, card.RightFaceValue) = (card.RightFaceValue, card.LeftFaceValue);
    }
    public IPlayer GetCurrentPlayer()
        => _currentPlayer ?? throw new InvalidOperationException("No current player set.");
    public void GetPlayableMoves(IPlayer player)
    {
        _placableCards.Clear();
        _placableCards.AddRange(_hand[player].Where(CanConnect));
    }
    public bool IsPlayable(ICard card, Side side)
    {
        return side == Side.LEFT
            ? card.LeftFaceValue == LeftEndValue || card.RightFaceValue == LeftEndValue
            : card.LeftFaceValue == RightEndValue || card.RightFaceValue == RightEndValue;
    }
    public bool IsPlayableCard(ICard card)
        => _placableCards.Contains(card);
    public List<ICard> GetPlacableCards()
        => _placableCards;
    public void ExecuteMove(ICard cardToPlace, Side placementSide)
    {
        _passCount = 0;
        _placementSide = placementSide;
        PlaceCardOnBoard(cardToPlace, _placementSide);
        _hand[_currentPlayer].Remove(cardToPlace);;
    }
    public void PlaceCardOnBoard(ICard card, Side side)
    {
        int? endValue = side == Side.LEFT ? LeftEndValue : RightEndValue;
        if ((side == Side.LEFT && card.LeftFaceValue == endValue) ||
            (side == Side.RIGHT && card.RightFaceValue == endValue))
        {
            FlipCard(card);
        }

        if (side == Side.LEFT)
        {
            _board.PlayedCards.Insert(0, card);
            UpdateEndValues(card.LeftFaceValue, Side.LEFT);
        }
        else
        {
            _board.PlayedCards.Add(card);
            UpdateEndValues(card.RightFaceValue, Side.RIGHT);
        }
    }
    public void NextTurn()
    {
        _round++;
        _turnIndex = (_turnIndex + 1) % _players.Count;
        _currentPlayer = _players[_turnIndex];
        OnPlayerTurn?.Invoke(_currentPlayer);
    }
    public void PassTurn()
    {
        _passCount++;
        NextTurn();
    }
    public List<ICard> GetBoard()
        => _board.PlayedCards;
    public List<ICard> GetHand()
        => _hand[_currentPlayer] ?? new List<ICard>();
    public int GetHandValue(IPlayer player)
    {
        int total = 0;
        foreach (ICard card in _hand[player])
        {
            if (card.IsDouble())
            {
                total += (card.LeftFaceValue == 0) ? 20 : card.LeftFaceValue * 2;
            }
            else
            {
                total += card.LeftFaceValue + card.RightFaceValue;
            }
        }
        return total;
    }
    public void CalculateScores()
    {
        _winScore = new int[_playerCount];
        foreach (IPlayer player in _players)
        {
            int value = GetHandValue(player);
            player.Score = value;
            _winScore[_players.IndexOf(player)] = player.Score;
        }
    }
    // public void SetScore(IPlayer player, int score)
    // {
    //     player.Score = score;
    // }
    public int[] GetScores()
        => _winScore ?? Array.Empty<int>();
    public int GetScore(IPlayer player)
        => player.Score;
    public IPlayer GetWinner()
    {
        IPlayer winner = _players[0];
        foreach (IPlayer player in _players) if (GetScore(player) < GetScore(winner)) winner = player;
        return winner;
    }
    public bool CheckGameOver()
    {
        if (isFullPass() || isAnyHandEmpty()) return true;
        return false;
    }
    private bool isFullPass()
        => _passCount >= _playerCount;
    private bool isAnyHandEmpty()
    {
        foreach (IPlayer player in _players) if (_hand[player].Count == 0) return true;
        return false;
    }
    private ICard createCard(int id, int left, int right)
    {
        return new Card(id, left, right);
    }
    public void GenerateStandardDeck()
    {
        for (int left = 0; left <= 6; left++)
        {
            for (int right = 0; right <= left; right++)
            {
                _deck.Cards.Add(createCard((left * 10) + right, left, right));
            }
        }
    }
    public void Shuffle()
    {
        Random random = new Random();
        List<ICard> cards = _deck.Cards;
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            ICard temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }
    public ICard DrawCard()
    {
        ICard drawnCard = _deck.Cards[0];
        _deck.Cards.RemoveAt(0);
        return drawnCard;
    }
}