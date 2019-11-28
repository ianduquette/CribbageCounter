using System;

namespace CribbageCounter {
    public class Card {
        private const string Ace = "A";
        private const string King = "K";
        private const string Queen = "Q";
        private const string Jack = "J";

        public string Value { get; }

        public Suit Suit { get; }

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

        public Card(string input) {
            validateInput(input);

            char suit = input[input.Length - 1];
            string value = input.Substring(0, input.Length - 1);

            validateSuit(suit);
            Suit = (Suit)suit;

            validateValue(value);
            Value = value;
        }

        public override string ToString() {
            return $"{Value}{((char)Suit)}";
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 23 + Value.GetHashCode();
                hash = hash * 23 + Suit.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object value) {
            if (Object.ReferenceEquals(null, value)) {
                return false;
            }

            if (Object.ReferenceEquals(this, value)) {
                return true;
            }

            if (value.GetType() != this.GetType()) {
                return false;
            }

            return IsEqual((Card)value);
        }

        public bool IsEqual(Card card) {
            return
                card.Value.Equals(this.Value, StringComparison.OrdinalIgnoreCase)
                && card.Suit == this.Suit;
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

        private void validateInput(string input) {
            if (input.Length < 2 || input.Length > 3) {
                throw new ArgumentException("Input must be in format of: 'AS', '10H', or '2C'");
            }
        }

        private void validateSuit(char suit) {
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

        private void validateValue(string value) {
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

    }
}