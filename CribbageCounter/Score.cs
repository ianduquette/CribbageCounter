using System;
using System.Collections.Generic;
using System.Linq;

namespace CribbageCounter {
    public class Score {

        private Card[] _cards;
        private Card[][] _allFive => new[] { _cards };
        private Card[][] _setsOfFour;
        private Card[][] _setsOfThree;
        private Card[][] _setsOfTwo;
        private IEnumerable<Card> _justTheHand => _cards.Take(4);

        public List<ScoreDetail> Details { get; } = new List<ScoreDetail>();
        public int TotalPoints => Details.Sum(d => d.Points);
        public ScoreDetail FourOfAKind => oneOf(ScoreDetailType.FourOfAKind);
        public ScoreDetail ThreeOfAKind => oneOf(ScoreDetailType.ThreeOfAKind);
        public List<ScoreDetail> Pairs => allOf(ScoreDetailType.Pair);
        public List<ScoreDetail> Runs => allOf(ScoreDetailType.Run);
        public ScoreDetail Flush => oneOf(ScoreDetailType.Flush);
        public List<ScoreDetail> Fifteens => allOf(ScoreDetailType.Fifteen);
        public ScoreDetail Nibs => oneOf(ScoreDetailType.Nibs);

        private Score(Card[] cards) {
            _cards = cards;
            initializeSetsOfFour();
            initializeSetsOfThree();
            initializeSetsOfTwo();
        }

        public static Score For(Card[] cards, bool isCrib) {
            var result = new Score(cards);
            result.calculateScore(isCrib);
            return result;
        }

        private void initializeSetsOfFour() {
            _setsOfFour = new Card[5][];
            _setsOfFour[0] = new Card[] { _cards[0], _cards[1], _cards[2], _cards[3] };
            _setsOfFour[1] = new Card[] { _cards[0], _cards[1], _cards[2], _cards[4] };
            _setsOfFour[2] = new Card[] { _cards[0], _cards[1], _cards[3], _cards[4] };
            _setsOfFour[3] = new Card[] { _cards[0], _cards[2], _cards[3], _cards[4] };
            _setsOfFour[4] = new Card[] { _cards[1], _cards[2], _cards[3], _cards[4] };
        }

        private void initializeSetsOfThree() {
            _setsOfThree = new Card[10][];
            _setsOfThree[0] = new Card[] { _cards[0], _cards[1], _cards[2] };
            _setsOfThree[1] = new Card[] { _cards[0], _cards[1], _cards[3] };
            _setsOfThree[2] = new Card[] { _cards[0], _cards[1], _cards[4] };
            _setsOfThree[3] = new Card[] { _cards[0], _cards[2], _cards[3] };
            _setsOfThree[4] = new Card[] { _cards[0], _cards[2], _cards[4] };
            _setsOfThree[5] = new Card[] { _cards[0], _cards[3], _cards[4] };
            _setsOfThree[6] = new Card[] { _cards[1], _cards[2], _cards[3] };
            _setsOfThree[7] = new Card[] { _cards[1], _cards[2], _cards[4] };
            _setsOfThree[8] = new Card[] { _cards[1], _cards[3], _cards[4] };
            _setsOfThree[9] = new Card[] { _cards[2], _cards[3], _cards[4] };
        }

        private void initializeSetsOfTwo() {
            _setsOfTwo = new Card[10][];
            _setsOfTwo[0] = new Card[] { _cards[0], _cards[1] };
            _setsOfTwo[1] = new Card[] { _cards[0], _cards[2] };
            _setsOfTwo[2] = new Card[] { _cards[0], _cards[3] };
            _setsOfTwo[3] = new Card[] { _cards[0], _cards[4] };
            _setsOfTwo[4] = new Card[] { _cards[1], _cards[2] };
            _setsOfTwo[5] = new Card[] { _cards[1], _cards[3] };
            _setsOfTwo[6] = new Card[] { _cards[1], _cards[4] };
            _setsOfTwo[7] = new Card[] { _cards[2], _cards[3] };
            _setsOfTwo[8] = new Card[] { _cards[2], _cards[4] };
            _setsOfTwo[9] = new Card[] { _cards[3], _cards[4] };
        }

        private void calculateScore(bool isCrib) {
            //Four of a kind
            scoreSets(_setsOfFour, allValuesEqual, addFourOfAKind);
            //Three of a kinds
            scoreSets(_setsOfThree, allValuesEqual, addThreeOfAKind);
            //Pairs
            scoreSets(_setsOfTwo, allValuesEqual, addPair);
            //Run of five cards
            scoreSets(_allFive, allValuesIncrementByOne, addRun);
            //Runs of 4 cards
            scoreSets(_setsOfFour, allValuesIncrementByOne, addRun);
            //Runs of 3 cards
            scoreSets(_setsOfThree, allValuesIncrementByOne, addRun);
            //Flush of 5 cards
            scoreSets(_allFive, allSuitsEqual, addFlush);
            //Flush of 4 cards
            if (!isCrib) {
                scoreSets(new[] { _justTheHand }, allSuitsEqual, addFlush);
            }
            //Fifteen of 5 cards
            scoreSets(_allFive, allValuesEqualFifteen, addFifteen);
            //Fifteen of 4 cards
            scoreSets(_setsOfFour, allValuesEqualFifteen, addFifteen);
            //Fifteen of 3 cards
            scoreSets(_setsOfThree, allValuesEqualFifteen, addFifteen);
            //Fifteen of 2 cards
            scoreSets(_setsOfTwo, allValuesEqualFifteen, addFifteen);
            //Count Nibs
            scoreNibs();
        }

        private static void scoreSets(IEnumerable<IEnumerable<Card>> setsOfCards, Predicate<IEnumerable<Card>> condition, Action<IEnumerable<Card>> addToScore) {
            setsOfCards.ForEach(s => {
                if (condition(s)) {
                    addToScore(sortCards(s));
                }
            });
        }

        private void scoreNibs() {
            foreach (var card in _justTheHand) {
                if (card.IsJack && card.Suit == _cards[4].Suit) {
                    Details.Add(ScoreDetail.CreateNibs(card));
                }
            }
        }

        private void addFourOfAKind(IEnumerable<Card> cards) {
            Details.Add(ScoreDetail.CreateFourOfAKind(cards));
        }

        private void addThreeOfAKind(IEnumerable<Card> cards) {
            //If not already in a four of a kind
            if (FourOfAKind != null && FourOfAKind.Cards.Intersect(cards).Any()) {
                return;
            }

            Details.Add(ScoreDetail.CreateThreeOfAKind(cards));
        }

        private void addPair(IEnumerable<Card> cards) {
            //If not already in a four of a kind
            if (FourOfAKind != null && FourOfAKind.Cards.Intersect(cards).Any()) {
                return;
            }
            //If not already in a three of a kind
            if (ThreeOfAKind != null && ThreeOfAKind.Cards.Intersect(cards).Any()) {
                return;
            }

            Details.Add(ScoreDetail.CreatePair(cards));
        }

        private void addRun(IEnumerable<Card> cards) {
            bool allCardsInAnotherRun = false;
            //If not already in another run
            Runs.ForEach(r => {
                if (cards.All(c => r.Cards.Contains(c))) {
                    allCardsInAnotherRun = true;
                    return;
                }
            });

            if (!allCardsInAnotherRun) {
                Details.Add(ScoreDetail.CreateRun(cards));
            }
        }

        private void addFlush(IEnumerable<Card> cards) {
            //If not already in another flush
            if (Flush != null && Flush.Cards.Intersect(cards).Any()) {
                return;
            }

            Details.Add(ScoreDetail.CreateFlush(cards));
        }

        private void addFifteen(IEnumerable<Card> cards) {
            Details.Add(ScoreDetail.CreateFifteen(cards));
        }

        private ScoreDetail oneOf(ScoreDetailType scoreDetailType) {
            return Details.SingleOrDefault(d => d.ScoreDetailType == scoreDetailType);
        }

        private List<ScoreDetail> allOf(ScoreDetailType scoreDetailType) {
            return Details.Where(d => d.ScoreDetailType == scoreDetailType).ToList();
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
    }
}