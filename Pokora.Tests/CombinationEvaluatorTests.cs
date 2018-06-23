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
    }
}
