using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Pokora.Tests
{
    public class CardDrawingStatisticsTests
    {
        private CombinationEvaluator _combinationEvaluator;

        [SetUp]
        public void Setup()
        {
            _combinationEvaluator = new CombinationEvaluator();
        }

        [Test]
        public void Statistics_Analalys_Hands()
        {
            int simulationCount = 100000;
            var occurencies = new Dictionary<CombinationType, int>();
            occurencies.Add(CombinationType.RoyalFlush, 0);
            occurencies.Add(CombinationType.StraightFlush, 0);
            occurencies.Add(CombinationType.FourOfAKind, 0);
            occurencies.Add(CombinationType.FullHouse, 0);
            occurencies.Add(CombinationType.Flush, 0);
            occurencies.Add(CombinationType.Straight, 0);
            occurencies.Add(CombinationType.ThreeOfAKind, 0);
            occurencies.Add(CombinationType.TwoPair, 0);
            occurencies.Add(CombinationType.OnePair, 0);
            occurencies.Add(CombinationType.HighCard, 0);

            var deck = new Deck();
            var cards = new List<Card>();
            for (int i = 0; i < simulationCount; i++)
            {
                deck.Regroup();
                deck.Shuffle();

                cards.Clear();
                for (int j = 0; j < 7; j++)
                {
                    cards.Add(deck.Draw());
                }

                var cardCombination = _combinationEvaluator.EvaluateCards(cards);
                occurencies[cardCombination.Type]++;
            }
            var results = new List<string>();
            foreach (var occurence in occurencies)
            {
                results.Add($"{occurence.Key.ToString()} : {Math.Round((double)occurence.Value / simulationCount * 100, 3)}%");
            }

            Assert.IsTrue(true);
        }
    }
}
