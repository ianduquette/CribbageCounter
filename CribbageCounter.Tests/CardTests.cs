using NUnit.Framework;

namespace CribbageCounter.Tests {
    public class CardTests {

        [TestCase("A", 'S', 1)]
        [TestCase("2", 'S', 2)]
        [TestCase("3", 'S', 3)]
        [TestCase("4", 'S', 4)]
        [TestCase("5", 'S', 5)]
        [TestCase("6", 'S', 6)]
        [TestCase("7", 'S', 7)]
        [TestCase("8", 'S', 8)]
        [TestCase("9", 'S', 9)]
        [TestCase("10", 'S', 10)]
        public void CanCreateCard(string value, char suit, int rank) {
            //Arrange
            
            //Act
            var card = new Card(value, suit);

            //Assert
            Assert.AreEqual($"{value}{suit}",card.ToString());
            Assert.AreEqual(rank, card.Rank);

        }
    }
}