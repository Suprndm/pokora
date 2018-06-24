namespace Pokora.GameMechanisms.Rounds
{
    public class HandsRound : RoundBase
    {
        protected override string RoundName => "Hands";

        public HandsRound(int smallBlind, int bigBlind, Deck deck, TableCards cards, INotifier notifier) : base(smallBlind, bigBlind, deck, cards, notifier)
        {
        }

        public override void Setup()
        {
            Players[Players.Count - 2].Pay(SmallBlind);
            Players[Players.Count - 1].Pay(BigBlind);

            Deck.Shuffle();

            foreach (var player in Players)
            {
                player.GiveHand(Deck.Draw(), Deck.Draw());
            }
        }

    }
}
