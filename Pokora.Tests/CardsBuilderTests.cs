using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Pokora.Tests
{
    public class CardsBuilderTests
    {
        [Test]
        public void ShouldBuildCardsFromString()
        {
            var str = "7C KC 4H QH QD AS TC";

            var cards = CardsBuilder.BuildCardsFromString(str);

            Assert.AreEqual(7, cards.ElementAt(0).Value);
            Assert.AreEqual(2, cards.ElementAt(0).Color);

            Assert.AreEqual(13, cards.ElementAt(1).Value);
            Assert.AreEqual(2, cards.ElementAt(1).Color);

            Assert.AreEqual(4, cards.ElementAt(2).Value);
            Assert.AreEqual(1, cards.ElementAt(2).Color);

            Assert.AreEqual(12, cards.ElementAt(3).Value);
            Assert.AreEqual(1, cards.ElementAt(3).Color);

            Assert.AreEqual(12, cards.ElementAt(4).Value);
            Assert.AreEqual(4, cards.ElementAt(4).Color);

            Assert.AreEqual(14, cards.ElementAt(5).Value);
            Assert.AreEqual(3, cards.ElementAt(5).Color);

            Assert.AreEqual(10, cards.ElementAt(6).Value);
            Assert.AreEqual(2, cards.ElementAt(6).Color);
        }

        [Test]
        public void ShouldBuildStringFromCard()
        {
            var cards = new List<Card>();
            cards.Add(new Card(6, 1));
            cards.Add(new Card(14, 3));
            cards.Add(new Card(10, 2));
            cards.Add(new Card(3, 3));
            cards.Add(new Card(12, 2));
            cards.Add(new Card(8, 3));
            cards.Add(new Card(11, 1));

            var str = CardsBuilder.BuildStringFromCards(cards);

            Assert.AreEqual("6H AS TC 3S QC 8S JH", str);
        }

        [Test]
        public void ShouldBuildCardsFromAndToStringWithoutAnyLoss()
        {
            var str = "7C KC 4H QH QD AS TC";

            var cards = CardsBuilder.BuildCardsFromString(str);
            var newStr = CardsBuilder.BuildStringFromCards(cards);

            Assert.AreEqual(str, newStr);
        }
    }
}
