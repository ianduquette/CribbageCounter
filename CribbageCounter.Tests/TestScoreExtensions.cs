using System.Linq;
using System.Collections.Generic;

namespace CribbageCounter.Tests {
    public static class TestScoreExtensions {
        public static IEnumerable<ScoreDetail> WhereNotFifteen(this Score score) {
            return score.Details.Where(d => d.ScoreDetailType != ScoreDetailType.Fifteen);
        }

        public static bool Matches(this IEnumerable<Card> source, IEnumerable<Card> other) {
            return source.Count() == other.Count() && source.Intersect(other).Count() == other.Count();
        }
    }
}