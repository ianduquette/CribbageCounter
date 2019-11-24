using System;
using NUnit.Framework;

namespace CribbageCounter.Tests {
    public class CardTests {

        [TestCase("AS", "A", 1, 1, Suit.Spade)]
        [TestCase("2H", "2", 2, 2, Suit.Heart)]
        [TestCase("3C", "3", 3, 3, Suit.Club)]
        [TestCase("4D", "4", 4, 4, Suit.Diamond)]
        [TestCase("5S", "5", 5, 5, Suit.Spade)]
        [TestCase("6H", "6", 6, 6, Suit.Heart)]
        [TestCase("7C", "7", 7, 7, Suit.Club)]
        [TestCase("8D", "8", 8, 8, Suit.Diamond)]
        [TestCase("9S", "9", 9, 9, Suit.Spade)]
        [TestCase("10H", "10", 10, 10, Suit.Heart)]
        [TestCase("JC", "J", 11, 10, Suit.Club)]
        [TestCase("QD", "Q", 12, 10, Suit.Diamond)]
        [TestCase("KS", "K", 13, 10, Suit.Spade)]
        public void Create_CanCreateCard(string input, string value, int rank, int points, Suit suit) {
            //Act
            var card = new Card(input);

            //Assert
            Assert.AreEqual($"{value}{((char)suit)}", card.ToString());
            Assert.AreEqual(value, card.Value);
            Assert.AreEqual(rank, card.Rank);
            Assert.AreEqual(points, card.Points);
            Assert.AreEqual(suit, card.Suit);
        }

        [Test]
        public void CreateCard_InvalidSuit() {
            //Arrange
            char invalidSuit = 'X';

            //Act
            var ex = Assert.Throws<InvalidSuitException>(() => {
                new Card("AX");
            });

            //Assert
            Assert.That(ex.Message, Is.EqualTo($"Suit of '{invalidSuit}' is invalid."));
        }

        [TestCase("X")]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("14")]
        [TestCase("BB")]
        public void CreateCard_InvalidValue(string value) {
            //Act
            var ex = Assert.Throws<InvalidValueException>(() => {
                new Card(value + 'C');
            });

            //Assert
            Assert.That(ex.Message, Is.EqualTo($"Value of '{value}' is invalid."));
        }

        [TestCase("5")]
        [TestCase("35HS")]
        public void CreateCard_TestInvalidInput(string input) {
            //Act
            var ex = Assert.Throws<ArgumentException>(() => {
                new Card(input);
            });

            //Assert
            Assert.AreEqual("Input must be in format of: 'AS', '10H', or '2C'", ex.Message);
        }

        [TestCase("AH")]
        [TestCase("KC")]
        [TestCase("QS")]
        [TestCase("JD")]
        [TestCase("10H")]
        [TestCase("2D")]
        public void Equals_Equal(string input) {
            //Arrange
            var card1 = new Card(input);
            var card2 = new Card(input);

            //Act
            var equalsMethod = card1.Equals(card2);
            var equalsOperator = card1 == card2;
            var hashCode1 = card1.GetHashCode();
            var hashCode2 = card2.GetHashCode();

            //Assert
            Assert.IsTrue(equalsMethod, "Equals()");
            Assert.IsTrue(equalsMethod, "==");
            Assert.AreEqual(hashCode1, hashCode2, "GetHashCode()");
        }

        [TestCase("AH", "AD")]
        [TestCase("KC", "KS")]
        [TestCase("QS", "QD")]
        [TestCase("JD", "JH")]
        [TestCase("10H", "10S")]
        [TestCase("2D", "2C")]
        [TestCase("2D", "AD")]
        public void Equals_NotEqual(string input1, string input2) {
            //Arrange
            var card1 = new Card(input1);
            var card2 = new Card(input2);

            //Act
            var equalsMethod = card1.Equals(card2);
            var equalsOperator = card1 == card2;
            var hashCode1 = card1.GetHashCode();
            var hashCode2 = card2.GetHashCode();

            //Assert
            Assert.IsFalse(equalsMethod, "Equals()");
            Assert.IsFalse(equalsMethod, "==");
            Assert.AreNotEqual(hashCode1, hashCode2, "GetHashCode()");
        }

    }
}