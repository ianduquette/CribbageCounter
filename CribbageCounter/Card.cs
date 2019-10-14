namespace CribbageCounter {
    public class Card {

        private const string Ace = "A";

        public string Value { get; set; }

        public char Suit { get; set; }

        public int Rank { get {
            return getRank();
        }}

        public Card(string value, char suit) {
            Value = value;
            Suit = suit;
        }       

        public override string ToString() {
            return $"{Value}{Suit}";
        }

        private int getRank() {
            switch (Value) {
                case Ace:
                    return 1;
                default:
                    return int.Parse(Value);
            }                       
        }

    }
}
