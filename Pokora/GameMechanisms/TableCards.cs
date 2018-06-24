using System.Collections.Generic;
using System.Linq;

namespace Pokora.GameMechanisms
{
    public class TableCards
    {
        private readonly IList<Card> _cards;
        private readonly INotifier _notifier;

        public TableCards(INotifier notifier)
        {
            _notifier = notifier;
            _cards = new List<Card>();
        }

        public IReadOnlyCollection<Card> Cards => _cards.ToList();

        public void Reveal(Card card)
        {
            _cards.Add(card);
            _notifier.CardRevealed(card);
        }

        public void Clear()
        {
            _cards.Clear();
            _notifier.CardsCleared();
        }
    }
}
