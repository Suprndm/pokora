using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;

namespace Pokora.Tests
{
    public class CombinationEvaluatorTests
    {
        private CombinationEvaluator _combinationEvaluator;

        [SetUp]
        public void Setup()
        {
            _combinationEvaluator = new CombinationEvaluator();
        }

        [TestCase("2D QH TH 8C KH AH JH", "AH KH QH JH TH")]
        [TestCase("JC QH TH 8C KH AH JH", "AH KH QH JH TH")]
        [TestCase("8H QH TH 9H KH AH JH", "AH KH QH JH TH")]
        public void ShouldEvalRoyalFlush(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.RoyalFlush, combination.Type);
            Assert.AreEqual(expectedCombination , combinationString);
        }

        [TestCase("2D QH TH 8C KH 9H JH", "KH QH JH TH 9H")]
        [TestCase("8H QH TH 9H KH 7H JH", "KH QH JH TH 9H")]
        [TestCase("8H 3C AC 5C KH 2C 4C", "5C 4C 3C 2C AC")]
        [TestCase("AC KH QH JH TH 9H 4C", "KH QH JH TH 9H")]
        [TestCase("QD QH TH 8C KH 9H JH", "KH QH JH TH 9H")]
        public void ShouldEvalStraightFlush(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.StraightFlush, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }

        [TestCase("4H 4D 4C 4S 3S 2S KH", "4H 4D 4C 4S KH")]
        [TestCase("8H AC AD 9C AH 2C AS", "AC AD AH AS 9C")]
        public void ShouldEvalFourOfAKind(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.FourOfAKind, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }

        [TestCase("KH 4D KS 2S 4S 6S KD", "KH KS KD 4D 4S")]
        [TestCase("KH 4D KS 4H 4S 6S KD", "KH KS KD 4D 4H")]
        public void ShouldEvalFullHouse(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.FullHouse, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }

        [TestCase("9H TH 8H 7D JD 2H 3H", "TH 9H 8H 3H 2H")]
        public void ShouldEvalFlush(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.Flush, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }

        [TestCase("9H TH 8H 7D JD 2S 3H", "JD TH 9H 8H 7D")]
        public void ShouldEvalStraight(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.Straight, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }

        [TestCase("3D 2H KH 3H 8C 3S 9D", "3D 3H 3S KH 9D")]
        public void ShouldEvalThreeOfAKind(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.ThreeOfAKind, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }

        [TestCase("3D 2H KH 3H 8C 2S JS", "3D 3H 2H 2S KH")]
        [TestCase("3D 2H KH 3H 8C 2S KS", "KH KS 3D 3H 8C")]
        public void ShouldEvalTwoPairs(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.TwoPair, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }


        [TestCase("3D 2H KH 3H 8C TS JS", "3D 3H KH JS TS")]
        public void ShouldEvalOnePair(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.OnePair, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }

        [TestCase("3D 2H KH 9H 8C TS AS", "AS KH TS 9H 8C")]
        public void ShouldEvalHighCards(string str, string expectedCombination)
        {
            var cards = CardsBuilder.BuildCardsFromString(str);

            var combination = _combinationEvaluator.EvaluateCards(cards);
            var combinationString = CardsBuilder.BuildStringFromCards(combination.Cards);

            Assert.AreEqual(CombinationType.HighCard, combination.Type);
            Assert.AreEqual(expectedCombination, combinationString);
        }

        [Test]
        public void ShouldPutAScoreOnEachHand()
        {
            var hands = new List<string>();
            hands.Add("AH KH QH JH TH 2S 4S");// RoyalFlush
            hands.Add("AS KS QS JS TS 8D 7H");// RoyalFlush
            hands.Add("QH JH TH 9H 8H AH 7S");// StraightFlush
            hands.Add("AS 2S 3S 4S 5S 8D 7H");// StraightFlush
            hands.Add("KH KS KD KC 3S 2S 4S");// FourOfAKind
            hands.Add("KH KS KD KC 3S 2H 2S");// FourOfAKind
            hands.Add("JH JS JD JC 3S 2H 2S");// FourOfAKind
            hands.Add("AH AS AD 3C 4S 3H 2S");// FullHouse
            hands.Add("AH AS AD 2C 4S 2H 2S");// FullHouse
            hands.Add("KH KS KD AC AS 2H 2S");// FullHouse
            hands.Add("AD 4D KD 6D 7D 8D 9D");// Flush
            hands.Add("4S QS JS TS 9S 5S 2S");// Flush
            hands.Add("TH 9D 8C 7C 6C 2S AS");// Straight
            hands.Add("9D 8C 7C 6C 5S AS JD");// Straight
            hands.Add("AD 2S 3D 4S 5S 8D 7H");// Straight
            hands.Add("AS AD AC KS 4C 2C 5H");// ThreeOfAKind
            hands.Add("AS AD AC KS 4C 2C 3H");// ThreeOfAKind
            hands.Add("AS AD AC QS 4C 2C 3H");// ThreeOfAKind
            hands.Add("KS KD KC AS JC TC 3H");// ThreeOfAKind
            hands.Add("KS KD QS QD 6H 4H 3H");// TwoPairs
            hands.Add("KS KD QS QD 5H 4H 3H");// TwoPairs
            hands.Add("KS KD 2S 2D 5H 4H 3H");// TwoPairs
            hands.Add("QS QD JS JD 5H 4H 3H");// TwoPairs
            hands.Add("QS QD AS TD 5H 4H 3H");// OnePair
            hands.Add("QS QD JS KD 5H 4H 3H");// OnePair
            hands.Add("JS JD AS KD 5H 4H 3H");// OnePair
            hands.Add("AS QD TS KD 5H 4H 3H");// HighCards
            hands.Add("AS 7D TS KD 5H 4H 3H");// HighCards
            hands.Add("AS 7D 8S KD 5H 4H 3H");// HighCards
            hands.Add("AS 7D 8S JD 5H 4H 3H");// HighCards

            var results = new Dictionary<string, CardCombination>();
            foreach (var hand in hands)
            {
                var cards = CardsBuilder.BuildCardsFromString(hand);
                var combination = _combinationEvaluator.EvaluateCards(cards);
                results.Add(hand, combination);
            }

           var ordererResults = results.OrderByDescending(kvp => kvp.Value.Score).ToList();

            for (int i = 0; i < ordererResults.Count; i++)
            {
                Assert.AreEqual(hands[i], ordererResults[i].Key, $"{hands[i]} expected but {ordererResults[i].Key} actual");
            }
        }
    }
}
