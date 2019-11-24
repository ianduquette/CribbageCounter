using System.Collections.Generic;
using System.Linq;

namespace CribbageCounter {
    public class Score {

        public List<ScoreDetail> Details { get; private set; }

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

        public void AddFourOfAKind(Card[] cards) {
            Details.Add(ScoreDetail.FourOfAKind(cards));
        }

        public void AddThreeOfAKind(Card[] cards) {
            //If not already in a four of a kind
            if (FourOfAKind != null && FourOfAKind.Cards.Intersect(cards).Any()) {
                return;
            }

            Details.Add(ScoreDetail.ThreeOfAKind(cards));
        }

        public void AddPair(Card[] cards) {
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

    }
}