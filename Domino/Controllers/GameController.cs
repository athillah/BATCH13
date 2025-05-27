using Domino.Interfaces;
using Domino.Models;
using Domino.Enumerations;
using System;

namespace Domino.Controllers;

public class GameController
{
    private IDeck _deck;
    private IBoard _board;
    private List<IPlayer> _players;
    private Dictionary<IPlayer, List<ICard>> _hand;
    //private<int id, CardPlacement > MoveOptions;
    private IPlayer _currentPlayer;
    private int _playerCount;
    private int _maxPlayers;
    private int _winScore;

    Action OnGameStart;
    Action<IPlayer> OnPlayerTurn;
    Action<bool> OnGameOver;

    public Side PlacementSide;
    public bool IsFlipped;
    public int ConnectingValue;
    public bool CanPlace;
    public Side AllowedSide;
    public bool NeedsFlip;
    public int LeftEndValue;
    public int RightEndValue;

    public GameController(List<IPlayer> players, IDeck deck, IBoard board)
    {
        Console.WriteLine("GameController constructor called.");
        _players = players;
        _deck = deck;
        _board = board;
    }
    public void SetupPlayers(int playerCount)
    {
        _playerCount = playerCount;
    }
    public void AssignPlayerNames()
    {
        foreach (IPlayer player in _players)
        {
            player.Name = "Player " + (_players.IndexOf(player) + 1);
        }
    }
    public void StartGame(Action OnGameStart)
    {
        throw new NotImplementedException("GameController.StartGame is not implemented yet.");
    }
    public void SetHandCard(IPlayer player)
    {
        throw new NotImplementedException("GameController.SetHandCard is not implemented yet.");
    }
    public ICard GetHandCard(IPlayer player)
    {
        throw new NotImplementedException("GameController.GetHandCard is not implemented yet.");
    }
    public void TurnOrder()
    {
        throw new NotImplementedException("GameController.TurnOrder is not implemented yet.");
    }
    public IPlayer DetermineFirstPlayer()
    {
        throw new NotImplementedException("GameController.DetermineFirstPlayer is not implemented yet.");
    }
    public int GetLeftEndValue()
    {
        return LeftEndValue;
    }
    public int GetRightEndValue()
    {
        return RightEndValue;
    }
    public void UpdateEndValues()
    {
        throw new NotImplementedException("GameController.UpdateEndValue is not implemented yet.");
    }
    public bool CanPlaceCard(ICard card)
    {
        if (card.GetValue() == RightEndValue || card.GetValue() == LeftEndValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //public int CanConnect(int value)
    //public int GetConnectingValue(Side side)
    //public PlacementInfo CreatePlacementInfo(bool canPlace, Side side, bool needsFlip)
    //public void FlipCard(ICard card)
    //public Dictionary<~>int id, CardPlacement> GetPlayableMoves(IPlayer player)
    //public CardPlacement CreateCardPlacement(ICard card, Side side, bool flipped, int value)
    //public bool ExecuteMove(int option, Side placementSide)
    public void PlaceCardOnBoard(ICard card, Side side)
    {
        if (side == Side.LEFT)
        {
            if (card.LeftFaceValue == LeftEndValue)
            {
                RotateCard(card);
            }
            _board.PlayedCards.Insert(0, card);
            LeftEndValue = card.LeftFaceValue;
        }
        if (side == Side.RIGHT)
        {
            if (card.RightFaceValue == RightEndValue)
            {
                RotateCard(card);
            }
            _board.PlayedCards.Insert(_board.PlayedCards.Count, card);
            RightEndValue = card.RightFaceValue;
        }
    }
    public void RotateCard(ICard card)
    {
        int temp = card.RightFaceValue;
        card.RightFaceValue = card.LeftFaceValue;
        card.LeftFaceValue = temp;
    }
    //public NextTurn(Action<IPlayer> OnPlayerTurn)
    //public void PassTurn()
    //public void ShowBoard()
    //public void ShowHand(IPlayer Player)
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
    //public bool CheckGameOver()
    public ICard CreateCard(int id, int right, int left)
    {
        return new Card(id, right, left);
    }
    public void GenerateStandardDeck()
    {
        for (int i = 0; i <= 6; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                _deck.Cards.Add(CreateCard((j*10) +i, i, j));
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
