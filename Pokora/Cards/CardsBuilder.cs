using System.Collections.Generic;

namespace Pokora.Cards
{
    public static class CardsBuilder
    {
        public static IReadOnlyCollection<Card> BuildCardsFromString(string s)
        {
            var cards = new List<Card>();
            var split = s.Split(' ');
            for (int i = 0; i < split.Length; i++)
            {
                var cardString = split[i];
                var valueChar = cardString[0];
                int value = 0;
                if (valueChar == 'A')
                    value = 14;
                else if (valueChar == 'K')
                    value = 13;
                else if (valueChar == 'Q')
                    value = 12;
                else if (valueChar == 'J')
                    value = 11;
                else if (valueChar == 'T')
                    value = 10;
                else value = int.Parse(valueChar.ToString());

                int color = 0;
                var colorChar = cardString[1];
                if (colorChar == 'H')
                    color = 1;
                else if (colorChar == 'C')
                    color = 2;
                else if (colorChar == 'S')
                    color = 3;
                else if (colorChar == 'D')
                    color = 4;

                cards.Add(new Card(value, color));
            }

            return cards;
        }

        public static string BuildStringFromCards(IReadOnlyCollection<Card> cards)
        {
            IList<string> cardStrings = new List<string>();
            foreach (var card in cards)
            {
                string str;
                if (card.Value == 14)
                    str = "A";
                else if (card.Value == 13)
                    str = "K";
                else if (card.Value == 12)
                    str = "Q";
                else if (card.Value == 11)
                    str = "J";
                else if (card.Value == 10)
                    str = "T";
                else str = card.Value.ToString();

                if (card.Color == 1)
                    str += "H";
                else if (card.Color == 2)
                    str += "C";
                else if (card.Color == 3)
                    str += "S";
                else if (card.Color == 4)
                    str += "D";

                cardStrings.Add(str);
            }

            return string.Join(" ", cardStrings);
        }
    }
}
