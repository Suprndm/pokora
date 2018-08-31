using System.Collections.Generic;
using Pokora.Utils;

namespace Pokora
{
    public class Deck
    {
        private readonly List<Card> _cards;
        public IReadOnlyCollection<Card> Cards { get; }

        private readonly IList<Card> _drawnCards;
        private readonly IList<Card> _burntCards;

        public Deck()
        {
            _drawnCards = new List<Card>();
            _burntCards = new List<Card>();

            _cards = new List<Card>();
            for (int i = 2; i <= 14; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    _cards.Add(new Card(i,j));
                }
            }

            Cards = _cards;
        }

        public void Burn()
        {
            _burntCards.Add(_cards[0]);
            _cards.RemoveAt(0);
        }

        public Card Draw()
        {
            var card = _cards[0];
            _drawnCards.Add(card);
            _cards.RemoveAt(0);
            return card;
        }

        public void Regroup()
        {
            foreach (var drawnCard in _drawnCards)
            {
                _cards.Add(drawnCard);
            }

            foreach (var burntCard in _burntCards)
            {
                _cards.Add(burntCard);
            }

            _drawnCards.Clear();
            _burntCards.Clear();
        }

        public void Shuffle()
        {
            _cards.LightShuffle();
        }
    }
}
