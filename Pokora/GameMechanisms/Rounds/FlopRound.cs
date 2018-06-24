namespace Pokora.GameMechanisms.Rounds
{
    public class FlopRound : RoundBase
    {
        protected override string RoundName => "Flop";

        public override void Setup()
        {
            Deck.Burn();
            Cards.Reveal(Deck.Draw());
            Cards.Reveal(Deck.Draw());
            Cards.Reveal(Deck.Draw());
        }

        public FlopRound(int smallBlind, int bigBlind, Deck deck, TableCards cards, INotifier notifier) : base(smallBlind, bigBlind, deck, cards, notifier)
        {
        }
    }
}
