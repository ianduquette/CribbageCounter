using System.Collections.Generic;
using System.Linq;

namespace CribbageCounter {
    public class Score {

        public List<ScoreDetail> Details { get; } = new List<ScoreDetail>();
        public int TotalPoints => Details.Sum(d => d.Points);
        public ScoreDetail FourOfAKind => oneOf(ScoreDetailType.FourOfAKind);
        public ScoreDetail ThreeOfAKind => oneOf(ScoreDetailType.ThreeOfAKind);
        public List<ScoreDetail> Pairs => allOf(ScoreDetailType.Pair);
        public List<ScoreDetail> Runs => allOf(ScoreDetailType.Run);
        public ScoreDetail Flush => oneOf(ScoreDetailType.Flush);
        public List<ScoreDetail> Fifteens => allOf(ScoreDetailType.Fifteen);

        public void AddFourOfAKind(IEnumerable<Card> cards) {
            Details.Add(ScoreDetail.CreateFourOfAKind(cards));
        }

        public void AddThreeOfAKind(IEnumerable<Card> cards) {
            //If not already in a four of a kind
            if (FourOfAKind != null && FourOfAKind.Cards.Intersect(cards).Any()) {
                return;
            }

            Details.Add(ScoreDetail.CreateThreeOfAKind(cards));
        }

        public void AddPair(IEnumerable<Card> cards) {
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

        public void AddRun(IEnumerable<Card> cards) {
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

        public void AddFlush(IEnumerable<Card> cards) {
            //If not already in another flush
            if (Flush != null && Flush.Cards.Intersect(cards).Any()) {
                return;
            }

            Details.Add(ScoreDetail.CreateFlush(cards));
        }

        public void AddFifteen(IEnumerable<Card> cards) {
            Details.Add(ScoreDetail.CreateFifteen(cards));
        }

        private ScoreDetail oneOf(ScoreDetailType scoreDetailType) {
            return Details.SingleOrDefault(d => d.ScoreDetailType == scoreDetailType);
        }

        private List<ScoreDetail> allOf(ScoreDetailType scoreDetailType) {
            return Details.Where(d => d.ScoreDetailType == scoreDetailType).ToList();
        }

    }
}