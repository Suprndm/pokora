using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Pokora.Cards;
using Pokora.GameMechanisms;
using Pokora.IA;
using Pokora.IA.Quality;
using Pokora.Poker;

namespace Pokora.Tests
{
    public class QualityEvaluatorTests
    {
        private QualityEvaluator _qualityEvaluator;

        [SetUp]
        public void Setup()
        {
            _qualityEvaluator = new QualityEvaluator();
        }

        [Test]
        public void PairOfAceShouldBe1()
        {
            var str = "AH AS";

            var cards = CardsBuilder.BuildCardsFromString(str).ToList();

            var playerHand = new PlayerHand(cards[0], cards[1]);

            var quality = _qualityEvaluator.EvalQualityScore(playerHand, new List<Card>());

            Assert.AreEqual(1, Math.Round(quality, 1));
        }

        [Test]
        public void TwoAndThreeShouldBe0()
        {
            var str = "2H 3S";

            var cards = CardsBuilder.BuildCardsFromString(str).ToList();

            var playerHand = new PlayerHand(cards[0], cards[1]);

            var quality = _qualityEvaluator.EvalQualityScore(playerHand, new List<Card>());

            Assert.AreEqual(0, Math.Round(quality, 1));
        }

        [Test]
        public void ShouldProduceQualityBetweenZeroAndOne()
        {
            int simulationCount = 1000000;
            var qualityResults = new Dictionary<int, IList<double>>();

            var deck = new Deck();
            var cards = new List<Card>();
            var hand = new List<Card>();
            for (int i = 0; i < 3; i++)
            {
                var numberOfCards = i + 3;
                qualityResults.Add(numberOfCards, new List<double>());

                for (int u = 0; u < simulationCount; u++)
                {
                    deck.Regroup();
                    deck.Shuffle();

                    cards.Clear();
                    hand.Clear();


                    for (int j = 0; j < numberOfCards; j++)
                    {
                        cards.Add(deck.Draw());
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        hand.Add(deck.Draw());
                    }

                    var quality = _qualityEvaluator.EvalQualityScore(new PlayerHand(hand[0], hand[1]), cards);

                    qualityResults[numberOfCards].Add(quality);
                }
            }

            var str = String.Join("\n", qualityResults.Values.First().ToList().OrderBy(v => v).Select(v => v.ToString()));


            //foreach (var keyValuePair in scoreResults.ToList())
            //{
            //    var max = keyValuePair.Value.OrderByDescending(v => v).First();
            //    var min = keyValuePair.Value.OrderByDescending(v => v).Last();
            //    scoreRanges.Add(new ScoreRange(keyValuePair.Key, min, max));
            //}

            var logPath = System.IO.Path.GetTempFileName();
            var logFile = System.IO.File.Create(logPath);
            var logWriter = new System.IO.StreamWriter(logFile);
            logWriter.WriteLine(str);
            logWriter.Dispose();
        }
    }
}
