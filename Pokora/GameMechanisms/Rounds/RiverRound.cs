namespace Pokora.GameMechanisms.Rounds
{
    public class RiverRound : RoundBase
    {
        protected override string RoundName => "River";

        public override void Setup()
        {
            Deck.Burn();
            Cards.Reveal(Deck.Draw());
        }

        public RiverRound(int smallBlind, int bigBlind, Deck deck, TableCards cards, INotifier notifier) : base(smallBlind, bigBlind, deck, cards, notifier)
        {
        }
    }
}
