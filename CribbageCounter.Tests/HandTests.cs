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

        [TestCase("3S", "3H", "3D", "3C", "KH", new[] { "3S", "3H", "3C", "3D" }, Description = "Cards in position 0, 1, 2, 3")]
        [TestCase("9C", "9D", "9H", "AC", "9S", new[] { "9S", "9H", "9C", "9D" }, Description = "Cards in position 0, 1, 2, 4")]
        [TestCase("QH", "QD", "AH", "QS", "QC", new[] { "QS", "QH", "QC", "QD" }, Description = "Cards in position 0, 1, 3, 4")]
        [TestCase("KD", "JS", "KC", "KS", "KH", new[] { "KS", "KH", "KC", "KD" }, Description = "Cards in position 0, 2, 3, 4")]
        [TestCase("KC", "AC", "AD", "AH", "AS", new[] { "AS", "AH", "AC", "AD" }, Description = "Cards in position 1, 2, 3, 4")]
        public void CountScore_FourOfAKind(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            testHandForOneDetailOnly(new[] { c1, c2, c3, c4, c5 }, cardsInScore, 12, "Four of a Kind", (s) => s.FourOfAKind);
        }

        [TestCase("3C", "3D", "3H", "5H", "2C", new[] { "3H", "3C", "3D" }, Description = "Cards in position 0, 1, 3")]
        [TestCase("4S", "4H", "4D", "AC", "2H", new[] { "4S", "4H", "4D" }, Description = "Cards in position 0, 1, 2")]
        [TestCase("6H", "6D", "KS", "QH", "6C", new[] { "6H", "6C", "6D" }, Description = "Cards in position 0, 1, 4")]
        [TestCase("7D", "2D", "7S", "7H", "4C", new[] { "7S", "7H", "7D" }, Description = "Cards in position 0, 2, 3")]
        [TestCase("8S", "AD", "8C", "8H", "2D", new[] { "8S", "8H", "8C" }, Description = "Cards in position 0, 2, 4")]
        [TestCase("9H", "2C", "3H", "9C", "9S", new[] { "9S", "9H", "9C" }, Description = "Cards in position 0, 3, 4")]
        [TestCase("4H", "JH", "JS", "JD", "KD", new[] { "JS", "JH", "JD" }, Description = "Cards in position 1, 2, 3")]
        [TestCase("3S", "QD", "QS", "6C", "QH", new[] { "QS", "QH", "QD" }, Description = "Cards in position 1, 2, 4")]
        [TestCase("7C", "KC", "JH", "KD", "KS", new[] { "KS", "KC", "KD" }, Description = "Cards in position 1, 3, 4")]
        [TestCase("3C", "JS", "10S", "10H", "10C", new[] { "10S", "10H", "10C" }, Description = "Cards in position 2, 3, 4")]
        public void CountScore_ThreeOfAKind(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            testHandForOneDetailOnly(new[] { c1, c2, c3, c4, c5 }, cardsInScore, 6, "Three of a Kind", (s) => s.ThreeOfAKind);
        }

        [TestCase("AC", "AH", "2D", "JC", "7S", new[] { "AH", "AC" }, Description = "Cards in position 0, 1")]
        [TestCase("2S", "3D", "2H", "5S", "9H", new[] { "2S", "2H" }, Description = "Cards in position 0, 2")]
        [TestCase("3D", "AD", "QH", "3H", "7C", new[] { "3H", "3D" }, Description = "Cards in position 0, 3")]
        [TestCase("4H", "QC", "KC", "10C", "4S", new[] { "4S", "4H" }, Description = "Cards in position 0, 4")]
        [TestCase("2D", "5C", "5S", "9D", "7S", new[] { "5S", "5C" }, Description = "Cards in position 1, 2")]
        [TestCase("10H", "6C", "4C", "6D", "8H", new[] { "6C", "6D" }, Description = "Cards in position 1, 3")]
        [TestCase("JS", "7S", "4D", "8S", "7C", new[] { "7S", "7C" }, Description = "Cards in position 1, 4")]
        [TestCase("KH", "QH", "8D", "8C", "AS", new[] { "8C", "8D" }, Description = "Cards in position 2, 3")]
        [TestCase("JC", "KS", "9H", "2D", "9S", new[] { "9S", "9H" }, Description = "Cards in position 2, 4")]
        [TestCase("3D", "10S", "9H", "KC", "KH", new[] { "KH", "KC" }, Description = "Cards in position 3, 4")]
        public void CountScore_Pair(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            testHandForOneDetailOnly(new[] { c1, c2, c3, c4, c5 }, cardsInScore, 2, "Pair", (s) => s.Pairs[0]);
        }

        private static void testHandForOneDetailOnly(string[] cardsInHand, string[] cardsInScore, int expectedPoints, string expectedDescription, Func<Score, ScoreDetail> getDetail) {
            //Arrange
            var hand = Hand.Create(cardsInHand[0], cardsInHand[1], cardsInHand[2], cardsInHand[3], cardsInHand[4]);

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(expectedPoints, score.Points);
            var scoreDetail = getDetail(score);
            var expectedCardsInHand = cardsInScore.Select(c => new Card(c)).ToList();
            assertScoreDetailOrdered(scoreDetail, expectedPoints, expectedDescription, expectedCardsInHand);
        }

        [TestCase("QH", "KC", "9H", "JD", "10S", new[] { "9H", "10S", "JD", "QH", "KC" }, Description = "Run of five.  No fifteens.")]
        [TestCase("5C", "4H", "3D", "AS", "2S", new[] { "AS", "2S", "3D", "4H", "5C" }, Description = "Run of five.  One fifteen.")]
        public void CountScore_RunOfFive(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(5, score.Points);

            var scoreDetail = score.Runs.FirstOrDefault();
            var expectedCardsInHand = cardsInScore.Select(c => new Card(c)).ToList();
            assertScoreDetailOrdered(scoreDetail, 5, "Run", expectedCardsInHand);
        }

        [TestCase("2S", "JD", "8H", "10C", "9H", new[] { "8H", "9H", "10C", "JD" }, Description = "Run of Four.  No fifteens.")]
        [TestCase("AS", "3D", "2H", "4C", "7H", new[] { "AS", "2H", "3D", "4C" }, Description = "Run of Four.  No fifteens.")]
        public void CountScore_RunOfFour(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(4, score.Points);
            var scoreDetail = score.Runs.FirstOrDefault();
            var expectedCardsInHand = cardsInScore.Select(c => new Card(c)).ToList();
            assertScoreDetailOrdered(scoreDetail, 4, "Run", expectedCardsInHand);
        }

        [TestCase("2S", "9C", "AC", "10C", "8S", new[] { "8S", "9C", "10C" }, Description = "Run of Three.  No fifteens.")]
        [TestCase("AH", "2H", "JC", "3H", "KS", new[] { "AH", "2H", "3H" }, Description = "Run of Three.  Two fifteens.")]
        public void CountScore_RunOfThree(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(3, score.Points);
            var scoreDetail = score.Runs.FirstOrDefault();
            var expectedCardsInHand = cardsInScore.Select(c => new Card(c)).ToList();
            assertScoreDetailOrdered(scoreDetail, 3, "Run", expectedCardsInHand);
        }

        [Test]
        public void CountScore_TwoRunsOfFour() {
            //Arrange
            var expectedPair = new List<Card> { new Card("3H"), new Card("3C") };
            var expectedFirstRun = new List<Card> { new Card("AS"), new Card("2C"), new Card("3C"), new Card("4D") };
            var expectedSecondRun = new List<Card> { new Card("AS"), new Card("2C"), new Card("3H"), new Card("4D") };

            var hand = Hand.Create("3C", "AS", "4D", "2C", "3H");

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(10, score.Points);
            Assert.AreEqual(1, score.Pairs.Count);
            Assert.AreEqual(2, score.Runs.Count);

            assertScoreDetailOrdered(score.Pairs[0], 2, "Pair", expectedPair);
            assertScoreDetailOrdered(score.Runs[0], 4, "Run", expectedFirstRun);
            assertScoreDetailOrdered(score.Runs[1], 4, "Run", expectedSecondRun);
        }

        [Test]
        public void CountScore_TwoRunsOfThree() {
            //Arrange
            var expectedPair = new List<Card> { new Card("3H"), new Card("3C") };
            var expectedFirstRun = new List<Card> { new Card("AC"), new Card("2S"), new Card("3H") };
            var expectedSecondRun = new List<Card> { new Card("AC"), new Card("2S"), new Card("3C") };

            var hand = Hand.Create("3H", "3C", "AC", "2S", "5S");

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(8, score.Points);
            Assert.AreEqual(1, score.Pairs.Count);
            Assert.AreEqual(2, score.Runs.Count);

            assertScoreDetailOrdered(score.Pairs[0], 2, "Pair", expectedPair);
            assertScoreDetailOrdered(score.Runs[0], 3, "Run", expectedFirstRun);
            assertScoreDetailOrdered(score.Runs[1], 3, "Run", expectedSecondRun);
        }

        [Test]
        public void CountScore_RunOfThreeAndPair() {
            //Arrange
            var expectedPair = new List<Card> { new Card("7S"), new Card("7C") };
            var expectedThreeOfAKind = new List<Card> { new Card("QH"), new Card("QC"), new Card("QD") };

            var hand = Hand.Create("7C", "QD", "QC", "7S", "QH");

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(8, score.Points);
            Assert.AreEqual(1, score.Pairs.Count);

            assertScoreDetailOrdered(score.Pairs[0], 2, "Pair", expectedPair);
            assertScoreDetailOrdered(score.ThreeOfAKind, 6, "Three of a Kind", expectedThreeOfAKind);
        }

        [TestCase("KC", "QC", "AC", "10C", "7C", new[] { "AC", "7C", "10C", "QC", "KC" }, Description = "Flush of five.  No fifteens. No Runs")]
        [TestCase("2D", "JD", "10S", "4D", "8D", new[] { "2D", "4D", "8D", "JD" }, Description = "Flush of four.  No fifteens. No Runs")]
        public void CountScore_Flush(string c1, string c2, string c3, string c4, string c5, string[] cardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(cardsInScore.Count(), score.Points);
            var scoreDetail = score.Flush;
            var expectedCardsInHand = cardsInScore.Select(c => new Card(c)).ToList();
            assertScoreDetailOrdered(scoreDetail, cardsInScore.Count(), "Flush", expectedCardsInHand);
        }

        [Test]
        public void CountScore_FlushOfFourAndRunOfFive() {
            //Arrange
            var expectedFlush = new List<Card> { new Card("8H"), new Card("9H"), new Card("10H"), new Card("JH") };
            var expectedRun = new List<Card> { new Card("8H"), new Card("9H"), new Card("10H"), new Card("JH"), new Card("QD") };

            var hand = Hand.Create("JH", "8H", "10H", "9H", "QD");

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(9, score.Points);

            assertScoreDetailOrdered(score.Runs.SingleOrDefault(), 5, "Run", expectedRun);
            assertScoreDetailOrdered(score.Flush, 4, "Flush", expectedFlush);
        }

        private static void assertScoreDetailOrdered(ScoreDetail actualScoreDetail, int expectedPoints, string expectedDescription, IList<Card> expectedInScore) {
            Assert.NotNull(actualScoreDetail, "scoreDetail != null");
            Assert.AreEqual(expectedPoints, actualScoreDetail.Points, "Points");
            Assert.AreEqual(expectedDescription, actualScoreDetail.Description, "Description");
            assertAllCardsInScore(expectedInScore, actualScoreDetail.Cards);

        }

        private static void assertAllCardsInScore(IList<Card> expectedInScore, IList<Card> actualCards) {
            for (int i = 0; i < expectedInScore.Count(); i++) {
                Assert.AreEqual(expectedInScore[i], actualCards[i]);
            }
        }

    }
}