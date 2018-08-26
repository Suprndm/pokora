namespace Pokora.SpinAndGo
{
    public class User
    {
        private readonly Wallet _wallet;

        public User(double initialAmount = 0)
        {
            _wallet = new Wallet();
            _wallet.Receive(initialAmount);
        }

        public string Name { get; set; }

        public double Cash => _wallet.Amount;

        public void Pay(double amount)
        {
            _wallet.Take(amount);
        }

        public void Earn(double amount)
        {
            _wallet.Receive(amount);
        }

        public IPlayerController Controller { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
