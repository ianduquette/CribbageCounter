using NUnit.Framework;

namespace CribbageCounter.Tests {
    public class CardTests {

        [TestCase("A", 'S', 1, 1)]
        [TestCase("2", 'H', 2, 2)]
        [TestCase("3", 'C', 3, 3)]
        [TestCase("4", 'D', 4, 4)]
        [TestCase("5", 'S', 5, 5)]
        [TestCase("6", 'H', 6, 6)]
        [TestCase("7", 'C', 7, 7)]
        [TestCase("8", 'D', 8, 8)]
        [TestCase("9", 'S', 9, 9)]
        [TestCase("10", 'H', 10, 10)]
        [TestCase("J", 'C', 11, 10)]
        [TestCase("Q", 'D', 12, 10)]
        [TestCase("K", 'S', 13, 10)]
        public void CanCreateCard(string value, char suit, int rank, int points) {
            //Arrange

            //Act
            var card = new Card(value, suit);

            //Assert
            Assert.AreEqual($"{value}{suit}", card.ToString());
            Assert.AreEqual(value, card.Value);
            Assert.AreEqual(suit, card.Suit);
            Assert.AreEqual(rank, card.Rank);
            Assert.AreEqual(points, card.Points);

        }

        [Test]
        public void CreateCard_InvalidSuit() {
            //Arrange
            char invalidSuit = 'X';
            //Act
            var ex = Assert.Throws<InvalidSuitException>(() => {
                new Card("A", invalidSuit);
            });

            //Assert      
            Assert.AreEqual(1, 1);
            Assert.That(ex.Message, Is.EqualTo($"Suit of '{invalidSuit}' is invalid."));
        }

        [TestCase("X")]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("14")]
        public void CreateCard_InvalidValue(string value) {
            
            //Act
            var ex = Assert.Throws<InvalidValueException>(() => {
                new Card(value, 'C');
            });

            //Assert      
            Assert.AreEqual(1, 1);
            Assert.That(ex.Message, Is.EqualTo($"Value of '{value}' is invalid."));
        }
    }
}