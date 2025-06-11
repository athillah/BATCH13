using NUnit.Framework;
using Moq;
using Domino.Controllers;
using Domino.Interfaces;
using Domino.Enums;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Linq;

namespace Domino.Tests
{
    [TestFixture]
    public class Controller_Should
    {
        private Mock<IDeck> _deckMock;
        private Mock<IBoard> _boardMock;
        private List<Mock<IPlayer>> _playerMocks;
        private GameController _controller;
        private const int MaxHandSize = 7;

        [SetUp]
        public void Setup()
        {
            _deckMock = new Mock<IDeck>();
            _boardMock = new Mock<IBoard>();
            _boardMock.Setup(b => b.PlayedCards).Returns(new List<ICard>()); // FIX: setup PlayedCards

            _playerMocks = new List<Mock<IPlayer>>
            {
                new Mock<IPlayer>(),
                new Mock<IPlayer>(),
                new Mock<IPlayer>()
            };

            var deckCards = new List<ICard>();
            _deckMock.Setup(d => d.Cards).Returns(deckCards);
            _deckMock.Setup(d => d.IsEmpty()).Returns(() => !deckCards.Any());

            var players = _playerMocks.Select(m => m.Object).ToList();
            _controller = new GameController(players, _deckMock.Object, _boardMock.Object, MaxHandSize);
        }

        [Test]
        public void SetupPlayers_FillsHands()
        {
            var dummyCard = new Mock<ICard>().Object;
            var cards = _deckMock.Object.Cards;
            for (int i = 0; i < MaxHandSize * _playerMocks.Count; i++)
                cards.Add(dummyCard);

            _controller.SetupPlayers();

            foreach (var player in _playerMocks.Select(m => m.Object))
                ClassicAssert.AreEqual(MaxHandSize, _controller.GetHandByPlayer(player).Count);
        }

        [Test]
        public void DetermineFirstPlayer_PicksHighestDouble()
        {
            var doubleCard = new Mock<ICard>();
            doubleCard.Setup(c => c.IsDouble()).Returns(true);
            doubleCard.Setup(c => c.LeftFaceValue).Returns(6);
            doubleCard.Setup(c => c.RightFaceValue).Returns(6);

            var simpleCard = new Mock<ICard>();
            simpleCard.Setup(c => c.IsDouble()).Returns(false);
            simpleCard.Setup(c => c.LeftFaceValue).Returns(5);
            simpleCard.Setup(c => c.RightFaceValue).Returns(5);

            _controller.SetupPlayers();
            _controller.GetHandByPlayer(_playerMocks[0].Object).Add(doubleCard.Object);
            _controller.GetHandByPlayer(_playerMocks[1].Object).Add(simpleCard.Object);

            _controller.DetermineFirstPlayer();

            ClassicAssert.AreEqual(_playerMocks[0].Object, _controller.GetCurrentPlayer());
            ClassicAssert.IsTrue(_boardMock.Object.PlayedCards.Contains(doubleCard.Object));
        }

        [Test]
        public void ExecuteMove_PlacesPlayableCard()
        {
            var player = _playerMocks[0].Object;
            _controller.SetupPlayers();
            typeof(GameController).GetField("_currentPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_controller, player);
            _controller.LeftEndValue = 1;

            var card = new Mock<ICard>();
            card.Setup(c => c.GetValue()).Returns(new[] { 1, 2 });
            card.Setup(c => c.LeftFaceValue).Returns(1);
            card.Setup(c => c.RightFaceValue).Returns(2);

            _controller.GetHandByPlayer(player).Add(card.Object);
            _controller.GetPlayableMoves(player);

            _controller.ExecuteMove(card.Object, Side.LEFT);

            ClassicAssert.IsTrue(_boardMock.Object.PlayedCards.First() == card.Object);
            ClassicAssert.IsFalse(_controller.GetHandByPlayer(player).Contains(card.Object));
        }

        [Test]
        public void CheckGameOver_FullPass_ReturnsTrue()
        {
            var dummyCard = new Mock<ICard>().Object;
            var cards = _deckMock.Object.Cards;
            for (int i = 0; i < MaxHandSize * _playerMocks.Count; i++)
                cards.Add(dummyCard);

            _controller.SetupPlayers();
            _controller.GetHandByPlayer(_playerMocks[0].Object).Add(dummyCard);
            _controller.DetermineFirstPlayer();

            _playerMocks.ForEach(pm => _controller.PassTurn());
            ClassicAssert.IsTrue(_controller.CheckGameOver());
        }

        [Test]
        public void CheckGameOver_EmptyHand_ReturnsTrue()
        {
            var dummyCard = new Mock<ICard>().Object;
            var cards = _deckMock.Object.Cards;
            for (int i = 0; i < MaxHandSize * _playerMocks.Count; i++)
                cards.Add(dummyCard);

            _controller.SetupPlayers();
            var player = _playerMocks[0].Object;
            _controller.GetHandByPlayer(player).Clear();
            ClassicAssert.IsTrue(_controller.CheckGameOver());
        }
    }
}
