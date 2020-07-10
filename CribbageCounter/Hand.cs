using System;
using System.Collections.Generic;
using System.Linq;

namespace CribbageCounter {
    public class Hand {
        public Card[] Cards { get; }
        private Card[][] _allFive => new[] { Cards };
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
            //Four of a kind
            scoreSets(_setsOfFour, allValuesEqual, result.AddFourOfAKind, result);
            //Three of a kinds
            scoreSets(_setsOfThree, allValuesEqual, result.AddThreeOfAKind, result);
            //Pairs
            scoreSets(_setsOfTwo, allValuesEqual, result.AddPair, result);
            //Run of five cards
            scoreSets(_allFive, allValuesIncrementByOne, result.AddRun, result);
            //Runs of 4 cards
            scoreSets(_setsOfFour, allValuesIncrementByOne, result.AddRun, result);
            //Runs of 3 cards
            scoreSets(_setsOfThree, allValuesIncrementByOne, result.AddRun, result);
            //Flush of 5 cards
            scoreSets(_allFive, allSuitsEqual, result.AddFlush, result);
            //Flush of 4 cards
            scoreSets(_setsOfFour, allSuitsEqual, result.AddFlush, result);
            //Fifteen of 5 cards
            scoreSets(_allFive, allValuesEqualFifteen, result.AddFifteen, result);
            //Fifteen of 4 cards
            scoreSets(_setsOfFour, allValuesEqualFifteen, result.AddFifteen, result);
            //Fifteen of 3 cards
            scoreSets(_setsOfThree, allValuesEqualFifteen, result.AddFifteen, result);
            //Fifteen of 2 cards
            scoreSets(_setsOfTwo, allValuesEqualFifteen, result.AddFifteen, result);
            return result;
        }

        private static void scoreSets(IEnumerable<IEnumerable<Card>> setsOfCards, Predicate<IEnumerable<Card>> condition, Action<IEnumerable<Card>> addToScore, Score score) {
            setsOfCards.ForEach(s => {
                if (condition(s)) {
                    addToScore(sortCards(s));
                }
            });
        }

        private static bool allValuesEqual(IEnumerable<Card> cards) {
            var f = cards.First();
            return cards.All(c => c.Value.Equals(f.Value, StringComparison.OrdinalIgnoreCase));
        }

        private static bool allValuesIncrementByOne(IEnumerable<Card> cards) {
            var listOfCards = sortCards(cards).ToList();
            for (int i = 0; i < listOfCards.Count() - 1; i++) {
                if (listOfCards[i].Rank + 1 != listOfCards[i + 1].Rank) {
                    return false;
                }
            }
            return true;
        }

        private static bool allSuitsEqual(IEnumerable<Card> cards) {
            var f = cards.First();
            return cards.All(c => c.Suit == f.Suit);
        }

        private static bool allValuesEqualFifteen(IEnumerable<Card> cards) {
            return cards.Sum(c => c.Points) == 15;
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