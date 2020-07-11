using System;
using System.Linq;

namespace CribbageCounter {
    public class Hand {
        private bool _isCrib;
        public Card[] Cards { get; }

        public static Hand Create(string one, string two, string three, string four, string turnedUp) {
            return Create(one, two, three, four, turnedUp, false);
        }

        public static Hand CreateCrib(string one, string two, string three, string four, string turnedUp) {
            return Create(one, two, three, four, turnedUp, true);
        }

        private static Hand Create(string one, string two, string three, string four, string turnedUp, bool isCrib) {
            return new Hand(new Card(one), new Card(two), new Card(three), new Card(four), new Card(turnedUp), isCrib);
        }

        private Hand(Card one, Card two, Card three, Card four, Card turnedUpCard, bool isCrib) {
            _isCrib = isCrib;
            Cards = new Card[] { one, two, three, four, turnedUpCard };

            if (Cards.Distinct().Count() != 5) {
                throw new ArgumentException("Duplicate card");
            }
        }

        public Score CountScore() {
            return Score.For(Cards, _isCrib);
        }

    }
}