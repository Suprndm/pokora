namespace Pokora.GameMechanisms.Rounds
{
    public class FourStraightRound : RoundBase
    {
        protected override string RoundName => "FourStraight";

        public override void Setup()
        {
            Deck.Burn();
            Cards.Reveal(Deck.Draw());
        }

        public FourStraightRound(int smallBlind, int bigBlind, Deck deck, TableCards cards, INotifier notifier) : base(smallBlind, bigBlind, deck, cards, notifier)
        {
        }
    }
}
