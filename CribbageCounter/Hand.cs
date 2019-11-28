using System;
using System.Collections.Generic;
using System.Linq;

namespace CribbageCounter {
    public class Hand {
        public Card[] Cards { get; }
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

            initializeSetsOfFour();
            initializeSetsOfThree();
            initializeSetsOfTwo();
        }

        public Score CountScore() {
            var result = new Score();
            countKinds(_setsOfFour, result, result.AddFourOfAKind);
            countKinds(_setsOfThree, result, result.AddThreeOfAKind);
            countKinds(_setsOfTwo, result, result.AddPair);
            countRuns(result, new[] { Cards });
            countRuns(result, _setsOfFour);
            countRuns(result, _setsOfThree);
            return result;
        }

        private static void countKinds(IEnumerable<IEnumerable<Card>> s, Score score, Action<IEnumerable<Card>> addToScore) {
            s.ForEach(i => {
                i = sortCards(i);
                var f = i.First();
                if (i.All(c => c.Value.Equals(f.Value, StringComparison.OrdinalIgnoreCase))) {
                    addToScore(i);
                    return;
                }
            });
        }

        private void countRuns(Score score, IEnumerable<IEnumerable<Card>> setsOfCards) {
            setsOfCards.ForEach(s => {
                var currentSet = sortCards(s).ToList();
                for (int i = 0; i < s.Count() - 1; i++) {
                    if (currentSet[i].Rank + 1 != currentSet[i + 1].Rank) {
                        return;
                    }
                }
                score.AddRun(currentSet);
            });
        }

        private static IEnumerable<Card> sortCards(IEnumerable<Card> cards) {
            return cards.OrderBy(c => c.Rank).ThenBy(c =>
                c.Suit == Suit.Spade ? 1 :
                c.Suit == Suit.Heart ? 2 :
                c.Suit == Suit.Club ? 3 :
                4);
        }

        private void initializeSetsOfFour() {
            _setsOfFour = new Card[5][];
            _setsOfFour[0] = new Card[] { Cards[0], Cards[1], Cards[2], Cards[3] };
            _setsOfFour[1] = new Card[] { Cards[0], Cards[1], Cards[2], Cards[4] };
            _setsOfFour[2] = new Card[] { Cards[0], Cards[1], Cards[3], Cards[4] };
            _setsOfFour[3] = new Card[] { Cards[0], Cards[2], Cards[3], Cards[4] };
            _setsOfFour[4] = new Card[] { Cards[1], Cards[2], Cards[3], Cards[4] };
        }

        private void initializeSetsOfThree() {
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

        private void initializeSetsOfTwo() {
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