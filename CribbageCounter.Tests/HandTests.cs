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

        [TestCase("3S", "3H", "3D", "3C", "KH", new[] { "3S", "3H", "3C", "3D" }, Description = "Four of a kind. Cards in position 0, 1, 2, 3. No fifteens.")]
        [TestCase("9C", "9D", "9H", "AC", "9S", new[] { "9S", "9H", "9C", "9D" }, Description = "Four of a kind. Cards in position 0, 1, 2, 4. No fifteens.")]
        [TestCase("QH", "QD", "AH", "QS", "QC", new[] { "QS", "QH", "QC", "QD" }, Description = "Four of a kind. Cards in position 0, 1, 3, 4. No fifteens.")]
        [TestCase("KD", "JS", "KC", "KS", "KH", new[] { "KS", "KH", "KC", "KD" }, Description = "Four of a kind. Cards in position 0, 2, 3, 4. No fifteens.")]
        [TestCase("KC", "AC", "AD", "AH", "AS", new[] { "AS", "AH", "AC", "AD" }, Description = "Four of a kind. Cards in position 1, 2, 3, 4. No fifteens.")]
        public void CountScore_FourOfAKind(string c1, string c2, string c3, string c4, string c5, string[] expectedCardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            assertScoreDetail(score, 12, expectedCardsInScore, "Four of a Kind", s => s.FourOfAKind);
            Assert.AreEqual(12, score.TotalPoints);
        }

        [TestCase("3S", "3H", "3D", "7C", "10H", new[] { "3S", "3H", "3D" }, Description = "Three of a kind. Cards in position 0, 1, 2. No fifteens.")]
        [TestCase("4C", "4D", "JH", "4H", "2C", new[] { "4H", "4C", "4D" }, Description = "Three of a kind. Cards in position 0, 1, 3. No fifteens.")]
        [TestCase("6H", "6D", "KS", "QH", "6C", new[] { "6H", "6C", "6D" }, Description = "Three of a kind. Cards in position 0, 1, 4. No fifteens.")]
        [TestCase("7D", "2D", "7S", "7H", "4C", new[] { "7S", "7H", "7D" }, Description = "Three of a kind. Cards in position 0, 2, 3. No fifteens.")]
        [TestCase("8S", "AD", "8C", "8H", "2D", new[] { "8S", "8H", "8C" }, Description = "Three of a kind. Cards in position 0, 2, 4. No fifteens.")]
        [TestCase("9H", "2C", "3H", "9C", "9S", new[] { "9S", "9H", "9C" }, Description = "Three of a kind. Cards in position 0, 3, 4. No fifteens.")]
        [TestCase("4H", "JH", "JS", "JD", "KD", new[] { "JS", "JH", "JD" }, Description = "Three of a kind. Cards in position 1, 2, 3. No fifteens.")]
        [TestCase("3S", "QD", "QS", "6C", "QH", new[] { "QS", "QH", "QD" }, Description = "Three of a kind. Cards in position 1, 2, 4. No fifteens.")]
        [TestCase("7C", "KC", "JH", "KD", "KS", new[] { "KS", "KC", "KD" }, Description = "Three of a kind. Cards in position 1, 3, 4. No fifteens.")]
        [TestCase("3C", "JS", "10S", "10H", "10C", new[] { "10S", "10H", "10C" }, Description = "Three of a kind. Cards in position 2, 3, 4. No fifteens.")]
        public void CountScore_ThreeOfAKind(string c1, string c2, string c3, string c4, string c5, string[] expectedCardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            assertScoreDetail(score, 6, expectedCardsInScore, "Three of a Kind", s => s.ThreeOfAKind);
            Assert.AreEqual(6, score.TotalPoints);
        }

        [TestCase("AC", "AH", "2D", "JC", "7S", new[] { "AH", "AC" }, Description = "Pair. Cards in position 0, 1. No fifteens.")]
        [TestCase("2S", "3D", "2H", "5S", "9H", new[] { "2S", "2H" }, Description = "Pair. Cards in position 0, 2. No fifteens.")]
        [TestCase("3D", "AD", "QH", "3H", "7C", new[] { "3H", "3D" }, Description = "Pair. Cards in position 0, 3. No fifteens.")]
        [TestCase("4H", "QC", "KC", "10C", "4S", new[] { "4S", "4H" }, Description = "Pair. Cards in position 0, 4. No fifteens.")]
        [TestCase("2D", "5C", "5S", "9D", "7S", new[] { "5S", "5C" }, Description = "Pair. Cards in position 1, 2. No fifteens.")]
        [TestCase("10H", "6C", "4C", "6D", "8H", new[] { "6C", "6D" }, Description = "Pair. Cards in position 1, 3. No fifteens.")]
        [TestCase("JS", "7S", "4D", "10S", "7C", new[] { "7S", "7C" }, Description = "Pair. Cards in position 1, 4. No fifteens.")]
        [TestCase("KH", "QH", "8D", "8C", "AS", new[] { "8C", "8D" }, Description = "Pair. Cards in position 2, 3. No fifteens.")]
        [TestCase("JC", "KS", "9H", "2D", "9S", new[] { "9S", "9H" }, Description = "Pair. Cards in position 2, 4. No fifteens.")]
        [TestCase("3D", "10S", "9H", "KC", "KH", new[] { "KH", "KC" }, Description = "Pair. Cards in position 3, 4. No fifteens.")]
        public void CountScore_Pair(string c1, string c2, string c3, string c4, string c5, string[] expectedCardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            assertScoreDetail(score, 2, expectedCardsInScore, "Pair", s => s.Pairs.SingleOrDefault());
            Assert.AreEqual(2, score.TotalPoints);
        }

        [TestCase("QH", "KC", "9H", "JD", "10S", new[] { "9H", "10S", "JD", "QH", "KC" }, Description = "Run of five. Cards in position 0, 1, 2, 3, 4. No fifteens.")]
        [TestCase("5C", "4H", "3D", "AS", "2S", new[] { "AS", "2S", "3D", "4H", "5C" }, Description = "Run of five. Cards in position 0, 1, 2, 3, 4. One fifteen.")]
        public void CountScore_RunOfFive(string c1, string c2, string c3, string c4, string c5, string[] expectedCardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            assertScoreDetail(score, 5, expectedCardsInScore, "Run", s => s.Runs.SingleOrDefault());
            Assert.AreEqual(score.Fifteens.Sum(s => s.Points) + 5, score.TotalPoints);
        }

        [TestCase("8H", "9H", "10C", "JD", "2S", new[] { "8H", "9H", "10C", "JD" }, Description = "Run of Four. Cards in position 0, 1, 2, 3. No fifteens.")]
        [TestCase("AS", "3D", "2H", "6C", "4C", new[] { "AS", "2H", "3D", "4C" }, Description = "Run of Four. Cards in position 0, 1, 2, 4. One fifteen.")]
        [TestCase("KC", "JH", "AC", "10H", "QD", new[] { "10H", "JH", "QD", "KC" }, Description = "Run of Four. Cards in position 0, 1, 3, 4. No fifteens.")]
        [TestCase("10C", "2S", "JC", "QS", "9S", new[] { "9S", "10C", "JC", "QS" }, Description = "Run of Four. Cards in position 0, 2, 3, 4. No fifteens.")]
        [TestCase("4C", "9C", "8H", "10H", "JC", new[] { "8H", "9C", "10H", "JC" }, Description = "Run of Four. Cards in position 1, 2, 3, 4. No fifteens.")]
        public void CountScore_RunOfFour(string c1, string c2, string c3, string c4, string c5, string[] expectedCardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            assertScoreDetail(score, 4, expectedCardsInScore, "Run", s => s.Runs.SingleOrDefault());
            Assert.AreEqual(score.Fifteens.Sum(s => s.Points) + 4, score.TotalPoints);
        }

        [TestCase("10C", "8S", "9C", "4C", "KH", new[] { "8S", "9C", "10C" }, Description = "Run of Three. Cards in position 0, 1, 2. No fifteens.")]
        [TestCase("AH", "2H", "8C", "3H", "KS", new[] { "AH", "2H", "3H" }, Description = "Run of Three. Cards in position 0, 1, 3. One fifteen.")]
        [TestCase("JH", "KD", "2S", "AS", "QS", new[] { "JH", "QS", "KD" }, Description = "Run of Three. Cards in position 0, 1, 4. No fifteens.")]
        [TestCase("5H", "AH", "6S", "7C", "JD", new[] { "5H", "6S", "7C" }, Description = "Run of Three. Cards in position 0, 2, 3. One fifteens.")]
        [TestCase("QS", "2S", "KD", "AD", "JS", new[] { "JS", "QS", "KD" }, Description = "Run of Three. Cards in position 0, 2, 4. No fifteens.")]
        [TestCase("QD", "2S", "6H", "JH", "KD", new[] { "JH", "QD", "KD" }, Description = "Run of Three. Cards in position 0, 3, 4. No fifteens.")]
        [TestCase("AH", "8S", "9S", "10H", "3C", new[] { "8S", "9S", "10H" }, Description = "Run of Three. Cards in position 1, 2, 3. No fifteens.")]
        [TestCase("8D", "2D", "AC", "QH", "3H", new[] { "AC", "2D", "3H" }, Description = "Run of Three. Cards in position 1, 2, 4. One fifteen.")]
        [TestCase("2D", "10C", "4H", "QC", "JC", new[] { "10C", "JC", "QC" }, Description = "Run of Three. Cards in position 1, 3, 4. No fifteens.")]
        [TestCase("6C", "4D", "JD", "9H", "10S", new[] { "9H", "10S", "JD" }, Description = "Run of Three. Cards in position 2, 3, 4. No fifteens.")]
        public void CountScore_RunOfThree(string c1, string c2, string c3, string c4, string c5, string[] expectedCardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            assertScoreDetail(score, 3, expectedCardsInScore, "Run", s => s.Runs.SingleOrDefault());
            Assert.AreEqual(score.Fifteens.Sum(score => score.Points) + 3, score.TotalPoints);
        }

        [Test]
        public void CountScore_TwoRunsOfFour() {
            //Arrange
            var expectedPair = new List<Card> { new Card("3H"), new Card("3C") };
            var expectedFirstRun = new List<Card> { new Card("AS"), new Card("2C"), new Card("3C"), new Card("4D") };
            var expectedSecondRun = new List<Card> { new Card("AS"), new Card("2C"), new Card("3H"), new Card("4D") };

            var hand = Hand.Create("AS", "2C", "3C", "3H", "4D");

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(10, score.TotalPoints);
            Assert.AreEqual(1, score.Pairs.Count);
            Assert.AreEqual(2, score.Runs.Count);

            assertScoreDetailOrdered(score.Pairs.ElementAtOrDefault(0), 2, "Pair", expectedPair);
            assertScoreDetailOrdered(score.Runs.ElementAtOrDefault(0), 4, "Run", expectedFirstRun);
            assertScoreDetailOrdered(score.Runs.ElementAtOrDefault(1), 4, "Run", expectedSecondRun);
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
            Assert.AreEqual(8, score.TotalPoints);
            Assert.AreEqual(1, score.Pairs.Count);
            Assert.AreEqual(2, score.Runs.Count);

            assertScoreDetailOrdered(score.Pairs.ElementAtOrDefault(0), 2, "Pair", expectedPair);
            assertScoreDetailOrdered(score.Runs.ElementAtOrDefault(0), 3, "Run", expectedFirstRun);
            assertScoreDetailOrdered(score.Runs.ElementAtOrDefault(1), 3, "Run", expectedSecondRun);
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
            Assert.AreEqual(8, score.TotalPoints);
            Assert.AreEqual(1, score.Pairs.Count);

            assertScoreDetailOrdered(score.Pairs.ElementAtOrDefault(0), 2, "Pair", expectedPair);
            assertScoreDetailOrdered(score.ThreeOfAKind, 6, "Three of a Kind", expectedThreeOfAKind);
        }

        [TestCase("KC", "QC", "AC", "10C", "7C", false, new[] { "AC", "7C", "10C", "QC", "KC" }, Description = "Flush of five. Cards in position 0, 1, 2, 3, 4. No fifteens.")]
        [TestCase("2D", "JD", "8D", "4D", "10S", false, new[] { "2D", "4D", "8D", "JD" }, Description = "Flush of four. Cards in position 0, 1, 2, 3. No fifteens.")]
        [TestCase("4C", "JC", "KC", "8C", "2H", true, new string [] { }, Description = "Flush of four. Cards in position 0, 1, 2, 3. No score possible, it's a crib.")]
        [TestCase("KS", "AS", "6C", "10S", "2S", false, new string[] { }, Description = "Flush of four. Cards in position 0, 1, 2, 4. No score possible.")]
        [TestCase("4C", "6C", "10D", "8C", "2C", false, new string[] { }, Description = "Flush of four. Cards in position 0, 1, 3, 4. No score possible.")]
        [TestCase("JH", "8C", "6H", "KH", "10H", false, new string[] { }, Description = "Flush of four. Cards in position 0, 2, 3, 4. No score possible.")]
        [TestCase("7D", "QH", "10H", "4H", "2H", false, new string[] { }, Description = "Flush of four. Cards in position 1, 2, 3, 4. No score possible.")]
        public void CountScore_Flush(string c1, string c2, string c3, string c4, string c5, bool isCrib, string[] expectedCardsInScore) {
            //Arrange
            var hand = isCrib ? Hand.CreateCrib(c1, c2, c3, c4, c5) : Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            if (!expectedCardsInScore.Any()) {
                Assert.AreEqual(0, score.TotalPoints, "Total points.");
                Assert.AreEqual(0, score.Details.Count);
            }
            else {
                assertScoreDetail(score, expectedCardsInScore.Length, expectedCardsInScore, "Flush", s => s.Flush);
                Assert.AreEqual(expectedCardsInScore.Count(), score.TotalPoints);
            }
        }

        [Test]
        public void CountScore_FlushOfFourAndRunOfFive() {
            //Arrange
            var expectedFlush = new List<Card> { new Card("8H"), new Card("9H"), new Card("10H"), new Card("JH") };
            var expectedRun = new List<Card> { new Card("8H"), new Card("9H"), new Card("10H"), new Card("JH"), new Card("QD") };

            var hand = Hand.Create("8H", "9H", "10H", "JH", "QD");

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(9, score.TotalPoints);

            assertScoreDetailOrdered(score.Runs.SingleOrDefault(), 5, "Run", expectedRun);
            assertScoreDetailOrdered(score.Flush, 4, "Flush", expectedFlush);
        }

        [TestCase("AC", "2S", "3H", "4D", "5C", new[] { "AC", "2S", "3H", "4D", "5C" }, Description = "Fifteen of five cards. Cards in position 0, 1, 2, 3, 4. Run of five.")]
        [TestCase("3S", "AH", "5S", "6D", "2S", new[] { "AH", "3S", "5S", "6D" }, Description = "Fifteen of four cards. Cards in position 0, 1, 2, 3. Run of three.")]
        [TestCase("2D", "2H", "3C", "9D", "8S", new[] { "2H", "2D", "3C", "8S" }, Description = "Fifteen of four cards. Cards in position 0, 1, 2, 4. Pair.")]
        [TestCase("AS", "QS", "6S", "2H", "2C", new[] { "AS", "2H", "2C", "QS" }, Description = "Fifteen of four cards. Cards in position 0, 1, 3, 4. Pair.")]
        [TestCase("KH", "7C", "AH", "2S", "2D", new[] { "AH", "2S", "2D", "KH" }, Description = "Fifteen of four cards. Cards in position 0, 2, 3, 4. Pair.")]
        [TestCase("AC", "2S", "7C", "2D", "4D", new[] { "2S", "2D", "4D", "7C" }, Description = "Fifteen of four cards. Cards in position 1, 2, 3, 4. Pair.")]
        [TestCase("2D", "7C", "6C", "QS", "QH", new[] { "2D", "6C", "7C" }, Description = "Fifteen of three cards. Cards in position 0, 1, 2.")]
        [TestCase("4H", "2H", "KS", "9H", "JS", new[] { "2H", "4H", "9H" }, Description = "Fifteen of three cards. Cards in position 0, 1, 3.")]
        [TestCase("2S", "3D", "7H", "9S", "QS", new[] { "2S", "3D", "QS" }, Description = "Fifteen of three cards. Cards in position 0, 1, 4.")]
        [TestCase("JD", "6H", "4C", "AS", "7C", new[] { "AS", "4C", "JD" }, Description = "Fifteen of three cards. Cards in position 0, 2, 3.")]
        [TestCase("5D", "AH", "8H", "3D", "2H", new[] { "2H", "5D", "8H" }, Description = "Fifteen of three cards. Cards in position 0, 2, 4.")]
        [TestCase("3C", "9S", "2C", "7S", "5S", new[] { "3C", "5S", "7S" }, Description = "Fifteen of three cards. Cards in position 0, 3, 4.")]
        [TestCase("6H", "10C", "4S", "AD", "3C", new[] { "AD", "4S", "10C" }, Description = "Fifteen of three cards. Cards in position 1, 2, 3.")]
        [TestCase("KD", "8D", "4H", "3S", "9H", new[] { "3S", "4H", "8D" }, Description = "Fifteen of three cards. Cards in position 1, 2, 4.")]
        [TestCase("9H", "5S", "3S", "5D", "5C", new[] { "5S", "5C", "5D" }, Description = "Fifteen of three cards. Cards in position 1, 3, 4. Three of a Kind")]
        [TestCase("KD", "10S", "3C", "9H", "3H", new[] { "3H", "3C", "9H" }, Description = "Fifteen of three cards. Cards in position 2, 3, 4. Pair.")]
        [TestCase("6H", "9S", "10S", "QD", "7H", new[] { "6H", "9S" }, Description = "Fifteen of two cards. Cards in position 0, 1.")]
        [TestCase("8C", "3D", "7H", "AS", "10C", new[] { "7H", "8C" }, Description = "Fifteen of two cards. Cards in position 0, 2.")]
        [TestCase("5D", "AD", "10D", "6H", "2C", new[] { "5D", "10D" }, Description = "Fifteen of two cards. Cards in position 0, 3.")]
        [TestCase("JH", "7C", "9C", "2D", "5C", new[] { "5C", "JH" }, Description = "Fifteen of two cards. Cards in position 0, 4.")]
        [TestCase("4D", "5C", "QS", "2S", "7S", new[] { "5C", "QS" }, Description = "Fifteen of two cards. Cards in position 1, 2.")]
        [TestCase("AH", "5S", "3H", "KD", "8H", new[] { "5S", "KD" }, Description = "Fifteen of two cards. Cards in position 1, 3.")]
        [TestCase("QH", "6S", "4D", "10S", "9D", new[] { "6S", "9D" }, Description = "Fifteen of two cards. Cards in position 1, 4.")]
        [TestCase("10D", "QC", "8H", "7H", "AS", new[] { "7H", "8H" }, Description = "Fifteen of two cards. Cards in position 2, 3.")]
        [TestCase("3H", "9S", "5H", "8D", "JC", new[] { "5H", "JC" }, Description = "Fifteen of two cards. Cards in position 2, 4. Run of three.")]
        [TestCase("2C", "2S", "7H", "5D", "QH", new[] { "5D", "QH" }, Description = "Fifteen of two cards. Cards in position 3, 4. Pair.")]
        public void CountScore_Fifteen(string c1, string c2, string c3, string c4, string c5, string[] expectedCardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            assertScoreDetail(score, 2, expectedCardsInScore, "Fifteen", s => s.Fifteens.SingleOrDefault());
            Assert.AreEqual(score.WhereNotFifteen().Sum(s => s.Points) + 2, score.TotalPoints);
        }

        private class FifteenTests {
            static object[] MultipleFifteens = {
                new object[] {"7C","8H","8C","10C","AD", new string[][] {new string[] {"7C", "8H"}, new string[] {"7C", "8C"}}},
                new object[] {"AH","KC","6H","4C","5C", new string[][] {new string[] {"4C", "5C", "6H"}, new string[] {"5C", "KC"}, new string[] {"AH", "4C", "KC"} }},
                new object[] {"5H","5D","5S","5C","AC", new string[][] {new string[] {"5S", "5H", "5C"}, new string[] {"5S", "5H", "5D"}, new string[] {"5H", "5C", "5D"}, new string[] {"5S", "5C", "5D"} }},
                new object[] {"9D","9H","9S","6C","6D", new string[][] {new string[] {"6C", "9S"}, new string[] {"6C", "9H"}, new string[] {"6C", "9D"},new string[] { "6D", "9S"}, new string[] {"6D", "9H"}, new string[] {"6D", "9D"}}},
                new object[] {"5D","JD","5H","9S","5S", new string[][] {new string[] {"5S", "JD"}, new string[] {"5H", "JD"}, new string[] {"5D", "JD"},new string[] {"5S", "5H", "5D"}}}
            };
        }

        [TestCaseSource(typeof(FifteenTests), "MultipleFifteens")]
        public void CountScore_FifteenMultiple(string c1, string c2, string c3, string c4, string c5, string[][] expectedCardsInScore) {
            //Arrange
            var hand = Hand.Create(c1, c2, c3, c4, c5);

            //Act
            var score = hand.CountScore();

            //Assert
            Assert.AreEqual(expectedCardsInScore.Length, score.Fifteens.Count());
            Assert.AreEqual(expectedCardsInScore.Length * 2, score.Fifteens.Sum(s => s.Points));
            expectedCardsInScore.ForEach(ec => {
                var expected = ec.Select(c => new Card(c)).ToList();
                var actual = score.Fifteens.Single(f => f.Cards.Matches(expected)).Cards;
                Assert.NotNull(actual, $"Cards {String.Join(",", expected.Select(e => e.ToString()))} not found in score!");
                assertAllCardsInScore(expected, actual);
            });
        }

        private static void assertScoreDetail(Score actualScore, int expectedPoints, string[] expectedCardsInScore, string expectedDescription, Func<Score, ScoreDetail> getDetail) {

            var expectedCards = expectedCardsInScore.Select(c => new Card(c)).ToList();
            var actualScoreDetail = getDetail(actualScore);

            assertScoreDetailOrdered(actualScoreDetail, expectedPoints, expectedDescription, expectedCards);
        }

        private static void assertScoreDetailOrdered(ScoreDetail actualScoreDetail, int expectedPoints, string expectedDescription, IList<Card> expectedCards) {
            Assert.NotNull(actualScoreDetail, "scoreDetail != null");
            Assert.AreEqual(expectedPoints, actualScoreDetail.Points, "Points");
            Assert.AreEqual(expectedDescription, actualScoreDetail.Description, "Description");

            assertAllCardsInScore(expectedCards, actualScoreDetail.Cards);
        }

        private static void assertAllCardsInScore(IList<Card> expectedCards, IList<Card> actualCards) {
            for (int i = 0; i < expectedCards.Count(); i++) {
                Assert.AreEqual(expectedCards[i], actualCards[i]);
            }
        }
    }
}