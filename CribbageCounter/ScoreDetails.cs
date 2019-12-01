using System.Collections.Generic;
using System.Linq;

namespace CribbageCounter {
    public class ScoreDetail {
        public ScoreDetailType ScoreDetailType { get; }

        public int Points { get; }

        public string Description { get; }

        public IList<Card> Cards { get; }

        private ScoreDetail(ScoreDetailType scoreDetailType, int points, string description, IEnumerable<Card> cards) {
            ScoreDetailType = scoreDetailType;
            Points = points;
            Description = description;
            Cards = cards.ToList();
        }

        public static ScoreDetail FourOfAKind(IEnumerable<Card> cards) {
            return new ScoreDetail(ScoreDetailType.FourOfAKind, 12, "Four of a Kind", cards);
        }

        public static ScoreDetail ThreeOfAKind(IEnumerable<Card> cards) {
            return new ScoreDetail(ScoreDetailType.ThreeOfAKind, 6, "Three of a Kind", cards);
        }

        public static ScoreDetail Pair(IEnumerable<Card> cards) {
            return new ScoreDetail(ScoreDetailType.Pair, 2, "Pair", cards);
        }

        public static ScoreDetail Run(IEnumerable<Card> cards) {
            return new ScoreDetail(ScoreDetailType.Run, cards.Count(), "Run", cards);
        }

        public static ScoreDetail Flush(IEnumerable<Card> cards) {
            return new ScoreDetail(ScoreDetailType.Flush, cards.Count(), "Flush", cards);
        }

    }
}