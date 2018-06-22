using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Pokora.Tests
{
    class DeckTests
    {
        private Deck _deck;

        [SetUp]
        public void Setup()
        {
            _deck = new Deck();
        }

        [Test]
        public void ShouldHave_52_Cards()
        {
            Assert.AreEqual(52, _deck.Cards.Count);
        }

        [Test]
        public void ShouldNotHaveDuplicatedCards()
        {
            Assert.AreEqual(true, _deck.Cards.Any(card1 => _deck.Cards
                .Count(card2 => card1.Value == card2.Value && card1.Color == card2.Color) == 1));
        }

        [Test]
        public void ShouldHaveFourColorsForEachValue()
        {
            Assert.IsTrue(_deck.Cards.GroupBy(card => card.Value).All(group => group.Count() == 4));
        }

        [Test]
        public void ShuffleShouldAtLeastChangeACardOrder()
        {
            var initialDeck = _deck.Cards.ToList();

            _deck.Shuffle();

            Assert.IsTrue(initialDeck.Any(card => _deck.Cards.ToList().IndexOf(card) != initialDeck.IndexOf(card)));
        }

        [Test]
        public void ShuffleShouldPreserveAllTheCards()
        {
            var initialDeck = _deck.Cards.ToList();

            _deck.Shuffle();

            Assert.IsTrue(initialDeck.All(card => _deck.Cards.Contains(card)));
        }

        [Test]
        public void DrawShouldGiveACardFromTheDeck()
        {
            var initialDeck = _deck.Cards.ToList();

            var drawnCard = _deck.Draw();
            Assert.IsTrue(initialDeck.Contains(drawnCard));
        }

        [Test]
        public void DrawhShouldNeverGiveTwiceTheSameCard()
        {
            var drawnCards = new List<Card>();
            for (int i = 0; i < _deck.Cards.Count; i++)
            {
                drawnCards.Add(_deck.Draw());
            }

            Assert.AreEqual(true, drawnCards.Any(card1 => drawnCards
                            .Count(card2 => card1.Value == card2.Value && card1.Color == card2.Color) == 1));
        }

        [Test]
        public void DrawShouldRemoveTheCardFromTheDeck()
        {
            _deck.Draw();
            Assert.AreEqual(51, _deck.Cards.Count);

            _deck.Draw();
            Assert.AreEqual(50, _deck.Cards.Count);
        }

        [Test]
        public void BurnShouldRemoveTheCardFromTheDeck()
        {
            _deck.Burn();
            Assert.AreEqual(51, _deck.Cards.Count);

            _deck.Burn();
            Assert.AreEqual(50, _deck.Cards.Count);
        }

        [Test]
        public void RegroupShouldRecoverAllBurntAndDrawnCards()
        {
            _deck.Draw();
            _deck.Burn();
            _deck.Draw();
            _deck.Burn();

            _deck.Regroup();
            Assert.AreEqual(52, _deck.Cards.Count);
        }

        [Test]
        public void RegroupShouldHaveNoEffectIfDoneSuccessiveTimes()
        {
            _deck.Draw();
            _deck.Burn();
            _deck.Draw();
            _deck.Burn();

            _deck.Regroup();
            _deck.Regroup();
            _deck.Regroup();
            _deck.Regroup();
            Assert.AreEqual(52, _deck.Cards.Count);
        }
    }
}
