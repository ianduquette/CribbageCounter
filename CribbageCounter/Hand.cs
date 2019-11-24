using System;
using System.Collections.Generic;
using System.Linq;

namespace CribbageCounter {
    public class Hand {
        public Card[] Cards { get; private set; }
        private Card[][] _setsOfFour;
        private Card[][] _setsOfThree;
        private Card[][] _setsOfTwo;

        public static Hand Create(string one, string two, string three, string four, string turnedUp) {
            return new Hand(new Card(one), new Card(two), new Card(three), new Card(four), new Card(turnedUp));
        }

        private Hand(Card one, Card two, Card three, Card four, Card turnedUpCard) {
            Cards = new Card[] { one, two, three, four, turnedUpCard };

            if (Cards.Distinct().Count() != 5) {
                throw new ArgumentException("Duplicate card");
            }

            InitializeSetsOfFour();
            InitializeSetsOfThree();
            InitializeSetsOfTwo();
        }

        public Score CountScore() {
            var result = new Score();
            CountFourOfAKind(result);
            CountThreeOfAKind(result);
            CountPairs(result);
            return result;
        }

        private void CountFourOfAKind(Score score) {
            IterateAndScoreKind(_setsOfFour, score, score.AddFourOfAKind);
        }

        private void CountThreeOfAKind(Score score) {
            IterateAndScoreKind(_setsOfThree, score, score.AddThreeOfAKind);
        }

        private void CountPairs(Score score) {
            IterateAndScoreKind(_setsOfTwo, score, score.AddPair);
        }

        private static void IterateAndScoreKind(IEnumerable<IEnumerable<Card>> s, Score score, Action<Card[]> addToScore) {
            s.ForEach(i => {
                var f = i.First();
                if (i.All(c => c.Value.Equals(f.Value, StringComparison.OrdinalIgnoreCase))) {
                    addToScore(i.ToArray());
                    return;
                }
            });
        }

        private void InitializeSetsOfFour() {
            _setsOfFour = new Card[5][];
            _setsOfFour[0] = new Card[] { Cards[0], Cards[1], Cards[2], Cards[3] };
            _setsOfFour[1] = new Card[] { Cards[0], Cards[1], Cards[2], Cards[4] };
            _setsOfFour[2] = new Card[] { Cards[0], Cards[1], Cards[3], Cards[4] };
            _setsOfFour[3] = new Card[] { Cards[0], Cards[2], Cards[3], Cards[4] };
            _setsOfFour[4] = new Card[] { Cards[1], Cards[2], Cards[3], Cards[4] };
        }

        private void InitializeSetsOfThree() {
            _setsOfThree = new Card[10][];
            _setsOfThree[0] = new Card[] { Cards[0], Cards[1], Cards[2] };
            _setsOfThree[1] = new Card[] { Cards[0], Cards[1], Cards[3] };
            _setsOfThree[2] = new Card[] { Cards[0], Cards[1], Cards[4] };
            _setsOfThree[3] = new Card[] { Cards[0], Cards[2], Cards[3] };
            _setsOfThree[4] = new Card[] { Cards[0], Cards[2], Cards[4] };
            _setsOfThree[5] = new Card[] { Cards[0], Cards[3], Cards[4] };
            _setsOfThree[6] = new Card[] { Cards[1], Cards[2], Cards[3] };
            _setsOfThree[7] = new Card[] { Cards[1], Cards[2], Cards[4] };
            _setsOfThree[8] = new Card[] { Cards[1], Cards[3], Cards[4] };
            _setsOfThree[9] = new Card[] { Cards[2], Cards[3], Cards[4] };
        }

        private void InitializeSetsOfTwo() {
            _setsOfTwo = new Card[10][];
            _setsOfTwo[0] = new Card[] { Cards[0], Cards[1] };
            _setsOfTwo[1] = new Card[] { Cards[0], Cards[2] };
            _setsOfTwo[2] = new Card[] { Cards[0], Cards[3] };
            _setsOfTwo[3] = new Card[] { Cards[0], Cards[4] };
            _setsOfTwo[4] = new Card[] { Cards[1], Cards[2] };
            _setsOfTwo[5] = new Card[] { Cards[1], Cards[3] };
            _setsOfTwo[6] = new Card[] { Cards[1], Cards[4] };
            _setsOfTwo[7] = new Card[] { Cards[2], Cards[3] };
            _setsOfTwo[8] = new Card[] { Cards[2], Cards[4] };
            _setsOfTwo[9] = new Card[] { Cards[3], Cards[4] };
        }

    }
}