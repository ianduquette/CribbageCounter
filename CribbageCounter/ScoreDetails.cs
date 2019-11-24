namespace CribbageCounter {
    public class ScoreDetail {
        public ScoreDetailType ScoreDetailType { get; private set; }

        public int Points { get; private set; }

        public string Description { get; private set; }

        public Card[] Cards {
            get; private set;
        }

        private ScoreDetail(ScoreDetailType scoreDetailType, int points, string description, Card[] cards) {
            ScoreDetailType = scoreDetailType;
            Points = points;
            Description = description;
            Cards = cards;
        }

        public static ScoreDetail FourOfAKind(Card[] cards) {
            return new ScoreDetail(ScoreDetailType.FourOfAKind, 12, "Four of a Kind", cards);
        }

        public static ScoreDetail ThreeOfAKind(Card[] cards) {
            return new ScoreDetail(ScoreDetailType.ThreeOfAKind, 6, "Three of a Kind", cards);
        }

        public static ScoreDetail Pair(Card[] cards) {
            return new ScoreDetail(ScoreDetailType.Pair, 2, "Pair", cards);
        }

    }
}