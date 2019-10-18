namespace CribbageCounter {
    public class Card {

        private const string Ace = "A";
        private const string King = "K";
        private const string Queen = "Q";
        private const string Jack = "J";

        public string Value { get; set; }

        public char Suit { get; set; }

        public int Rank {
            get {
                return getRank();
            }
        }
        public int Points {
            get {
                return getPoints();
            }
        }

        public Card(string value, char suit) {
            ValidateValue(value);
            ValidateSuit(suit);
            Value = value;            
            Suit = suit;
        }

        public override string ToString() {
            return $"{Value}{Suit}";
        }

        private void ValidateSuit(char suit) {
            switch (suit) {
                case 'S':
                case 'H':
                case 'C':
                case 'D':
                    return;
                default:
                    throw new InvalidSuitException($"Suit of '{suit}' is invalid.");
            }
        }

         private void ValidateValue(string value) {             
            switch (value) {
                case "A":
                case "J":
                case "Q":
                case "K":
                    return;                
                default:
                    int cardValue;
                    bool success = int.TryParse(value, out cardValue);
                    if (success && cardValue >= 2 && cardValue <= 10) {
                        return;
                    }
                    throw new InvalidValueException($"Value of '{value}' is invalid.");
            }
        }

        private int getRank() {
            switch (Value) {
                case Ace:
                    return 1;
                case Jack:
                    return 11;
                case Queen:
                    return 12;
                case King:
                    return 13;
                default:
                    return int.Parse(Value);
            }
        }

        private int getPoints() {
            switch (Value) {
                case Ace:
                    return 1;
                case Jack:
                case Queen:
                case King:
                    return 10;
                default:
                    return int.Parse(Value);
            }
        }

    }
}
