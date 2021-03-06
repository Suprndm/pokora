﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Pokora.Cards;
using Pokora.IA.Quality;
using Pokora.Poker;

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
        public void ScoreRangeCalculator()
        {
            int simulationCount = 500000;
            var scoreResults = new Dictionary<int, IList<double>>();
            var scoreRanges = new List<ScoreRange>();

            var deck = new Deck();
            var cards = new List<Card>();
            var hand = new List<Card>();
            for (int i = 3; i < 4; i++)
            {
                var numberOfCards = i + 2;
                scoreResults.Add(numberOfCards, new List<double>());

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

                    var cardCombination = _combinationEvaluator.EvaluateCards(cards);



                    scoreResults[numberOfCards].Add(cardCombination.Score);
                }
            }

            var str = String.Join("\n", scoreResults.Values.First().ToList().OrderBy(v => v).Select(v=>v.ToString()));


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

        [Test]
        public void Statistics_Analalys_Hands()
        {
            int simulationCount = 1000000;
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

            var probabilities = new Dictionary<CombinationType, double>();

            foreach (var occurence in occurencies)
            {
                var proba = (double)occurence.Value / simulationCount;
                results.Add($"{occurence.Key.ToString()} : {Math.Round(proba * 100, 3)}%");

                probabilities.Add(occurence.Key, proba);
            }

            CheckProba(CombinationType.RoyalFlush, probabilities[CombinationType.RoyalFlush], 0.00002, 0.00004);
            CheckProba(CombinationType.StraightFlush, probabilities[CombinationType.StraightFlush], 0.0002, 0.0004);
            CheckProba(CombinationType.FourOfAKind, probabilities[CombinationType.FourOfAKind], 0.0015, 0.002);
            CheckProba(CombinationType.FullHouse, probabilities[CombinationType.FullHouse], 0.02, .03);
            CheckProba(CombinationType.Flush, probabilities[CombinationType.Flush], 0.025, 0.035);
            CheckProba(CombinationType.Straight, probabilities[CombinationType.Straight], 0.042, 0.049);
            CheckProba(CombinationType.ThreeOfAKind, probabilities[CombinationType.ThreeOfAKind], 0.042, 0.05);
            CheckProba(CombinationType.TwoPair, probabilities[CombinationType.TwoPair], 0.20, 0.25);
            CheckProba(CombinationType.OnePair, probabilities[CombinationType.OnePair], 0.40, 0.45);
            CheckProba(CombinationType.HighCard, probabilities[CombinationType.HighCard], 0.15, 0.19);
        }

        private void CheckProba(CombinationType combinationType, double currentProba, double low, double high)
        {
            Assert.IsTrue(currentProba >= low && currentProba <= high, $"{combinationType.ToString()} : {currentProba} is not betwen {low} and {high}");
        }


        [Test]
        public void Statistics_Score_Hands()
        {
            int simulationCount = 1000000;
            var cardCombinations = new List<CardCombination>();

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
                cardCombinations.Add(cardCombination);
            }

            cardCombinations = cardCombinations.OrderByDescending(combination => combination.Score).ToList();
            var previousCombinationType = (int)CombinationType.RoyalFlush;
            CardCombination previousCombination = null;
            foreach (var cardCombination in cardCombinations)
            {
                if ((int)cardCombination.Type > previousCombinationType)
                {
                    var cardsString = CardsBuilder.BuildStringFromCards(cardCombination.Cards);
                    var previousCardsString = CardsBuilder.BuildStringFromCards(cardCombination.Cards);
                    throw new Exception(
                        $"{cardsString} is a {cardCombination.Type} with {cardCombination.Score} that is lower in score than {previousCardsString}-{previousCombination.Type}-{previousCombination.Score}");
                }

                previousCombination = cardCombination;
                previousCombinationType = (int)cardCombination.Type;
            }

        }
    }
}
