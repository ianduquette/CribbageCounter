using System.Collections.Generic;
using System.Linq;

namespace CribbageCounter {
    public class Score {

        public List<ScoreDetail> Details { get; }

        public int Points {
            get {
                return Details.Sum(d => d.Points);
            }
        }
        public Score() {
            Details = new List<ScoreDetail>();
        }

        public ScoreDetail FourOfAKind {
            get {
                return Details.SingleOrDefault(d => d.ScoreDetailType == ScoreDetailType.FourOfAKind);
            }
        }

        public ScoreDetail ThreeOfAKind {
            get {
                return Details.SingleOrDefault(d => d.ScoreDetailType == ScoreDetailType.ThreeOfAKind);
            }
        }

        public List<ScoreDetail> Pairs {
            get {
                return Details.Where(d => d.ScoreDetailType == ScoreDetailType.Pair).ToList();
            }
        }

        public List<ScoreDetail> Runs {
            get {
                return Details.Where(d => d.ScoreDetailType == ScoreDetailType.Run).ToList();
            }
        }

        public void AddFourOfAKind(IEnumerable<Card> cards) {
            Details.Add(ScoreDetail.FourOfAKind(cards));
        }

        public void AddThreeOfAKind(IEnumerable<Card> cards) {
            //If not already in a four of a kind
            if (FourOfAKind != null && FourOfAKind.Cards.Intersect(cards).Any()) {
                return;
            }

            Details.Add(ScoreDetail.ThreeOfAKind(cards));
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

            Details.Add(ScoreDetail.Pair(cards));
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
                Details.Add(ScoreDetail.Run(cards));
            }
        }

    }
}