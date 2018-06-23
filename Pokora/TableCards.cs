using System.Collections.Generic;
using System.Linq;

namespace Pokora
{
    public class TableCards
    {
        private readonly IList<Card> _cards;

        public TableCards()
        {
            _cards = new List<Card>();
        }

        public IReadOnlyCollection<Card> Cards => _cards.ToList();

        public void Reveal(Card card)
        {
            _cards.Add(card);
        }

        public void Clear()
        {
            _cards.Clear();
        }
    }
}
