using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace CribbageCounter.Tests {
    public class HandTests {

        [Test]
        public void Create_CannotAddDuplicateCards() {
            //Act
            var ex = Assert.Throws<ArgumentException>(() => {
                Hand.Create("AS", "AC", "AC", "2H", "3D");
            });

            //Assert
            Assert.AreEqual("Duplicate card", ex.Message);
        }

        [TestCase("3S", "3H", "3D", "3C", "KH", new[] { "3S", "3H", "3D", "3C" }, Description = "Cards in position 0, 1, 2, 3")]
        [TestCase("9C", "9D", "9H", "AC", "9S", new[] { "9C", "9D", "9H", "9S" }, Description = "Cards in position 0, 1, 2, 4")]
        [TestCase("QH", "QD", "AH", "QS", "QC", new[] { "QH", "QD", "QS", "QC" }, Description = "Cards in position 0, 1, 3, 4")]
        [TestCase("KD", "JS", "KC", "KS", "KH", new[] { "KD", "KC", "KS", "KH" }, Description = "Cards in position 0, 2, 3, 4")]
        [TestCase("KC", "AC", "AD", "AH", "AS", new[] { "AC", "AD", "AH", "AS" }, Description = "Cards in position 1, 2, 3, 4")]
        public void CountScore_FourOfAKind(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            TestHandForOneDetail(new[] { c1, c2, c3, c4, c5 }, cardsInScore, 12, "Four of a Kind", (s) => s.FourOfAKind);
        }

        [TestCase("3C", "3D", "3H", "5H", "2C", new[] { "3C", "3D", "3H" }, Description = "Cards in position 0, 1, 3")]
        [TestCase("4S", "4H", "4D", "AC", "2H", new[] { "4S", "4H", "4D" }, Description = "Cards in position 0, 1, 2")]
        [TestCase("6H", "6D", "KS", "QH", "6C", new[] { "6H", "6D", "6C" }, Description = "Cards in position 0, 1, 4")]
        [TestCase("7D", "2D", "7S", "7H", "4C", new[] { "7D", "7S", "7H" }, Description = "Cards in position 0, 2, 3")]
        [TestCase("8S", "AD", "8C", "8H", "2D", new[] { "8S", "8C", "8H" }, Description = "Cards in position 0, 2, 4")]
        [TestCase("9H", "2C", "3H", "9C", "9S", new[] { "9H", "9C", "9S" }, Description = "Cards in position 0, 3, 4")]
        [TestCase("4H", "JH", "JS", "JD", "KD", new[] { "JH", "JS", "JD" }, Description = "Cards in position 1, 2, 3")]
        [TestCase("3S", "QD", "QS", "6C", "QH", new[] { "QD", "QS", "QH" }, Description = "Cards in position 1, 2, 4")]
        [TestCase("7C", "KC", "JH", "KD", "KS", new[] { "KC", "KD", "KS" }, Description = "Cards in position 1, 3, 4")]
        [TestCase("3C", "JS", "10S", "10H", "10C", new[] { "10S", "10H", "10C" }, Description = "Cards in position 2, 3, 4")]
        public void CountScore_ThreeOfAKind(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            TestHandForOneDetail(new[] { c1, c2, c3, c4, c5 }, cardsInScore, 6, "Three of a Kind", (s) => s.ThreeOfAKind);
        }

        [TestCase("AC", "AH", "2D", "JC", "7S", new[] { "AC", "AH" }, Description = "Cards in position 0, 1")]
        [TestCase("2S", "3D", "2H", "5S", "9H", new[] { "2S", "2H" }, Description = "Cards in position 0, 2")]
        [TestCase("3D", "AD", "QH", "3H", "7C", new[] { "3D", "3H" }, Description = "Cards in position 0, 3")]
        [TestCase("4H", "QC", "KC", "10C", "4S", new[] { "4H", "4S" }, Description = "Cards in position 0, 4")]
        [TestCase("2D", "5C", "5S", "9D", "7S", new[] { "5C", "5S" }, Description = "Cards in position 1, 2")]
        [TestCase("10H", "6C", "4C", "6D", "8H", new[] { "6C", "6D" }, Description = "Cards in position 1, 3")]
        [TestCase("JS", "7S", "4D", "8S", "7C", new[] { "7S", "7C" }, Description = "Cards in position 1, 4")]
        [TestCase("KH", "QH", "8D", "8C", "AS", new[] { "8D", "8C" }, Description = "Cards in position 2, 3")]
        [TestCase("JC", "KS", "9H", "2D", "9S", new[] { "9H", "9S" }, Description = "Cards in position 2, 4")]
        [TestCase("3D", "10S", "9H", "KC", "KH", new[] { "KC", "KH" }, Description = "Cards in position 3, 4")]
        public void CountScore_Pair(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            TestHandForOneDetail(new[] { c1, c2, c3, c4, c5 }, cardsInScore, 2, "Pair", (s) => s.Pairs[0]);
        }

        private static void TestHandForOneDetail(string[] cardsInHand, string[] cardsInScore, int expectedPoints, string expectedDescription, Func<Score, ScoreDetail> getDetail) {
            //Arrange
            var hand = Hand.Create(cardsInHand[0], cardsInHand[1], cardsInHand[2], cardsInHand[3], cardsInHand[4]);

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(expectedPoints, score.Points);
            var scoreDetail = getDetail(score);
            Card[] expectedCardsInHand = cardsInScore.Select(c => new Card(c)).ToArray();
            AssertScoreDetail(scoreDetail, expectedPoints, expectedDescription, expectedCardsInHand);
        }

        private static void AssertScoreDetail(ScoreDetail actualScoreDetail, int expectedPoints, string expectedDescription, Card[] expectedInScore) {
            Assert.NotNull(actualScoreDetail, "scoreDetail != null");
            Assert.AreEqual(expectedPoints, actualScoreDetail.Points, "Points");
            Assert.AreEqual(expectedDescription, actualScoreDetail.Description, "Description");

            expectedInScore.ForEach(c => {
                Assert.Contains(c, actualScoreDetail.Cards, "Cards");
            });
        }

    }
}