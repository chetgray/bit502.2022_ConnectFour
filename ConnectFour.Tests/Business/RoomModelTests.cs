using ConnectFour.Business.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.Business
{
    [TestClass]
    public class RoomModelTests
    {
        [TestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(2, 2)]
        [DataRow(3, 1)]
        [DataRow(3, 2)]
        [DataRow(3, 3)]
        [DataRow(4, 1)]
        [DataRow(4, 2)]
        [DataRow(4, 3)]
        [DataRow(4, 4)]
        [DataRow(5, 2)]
        [DataRow(5, 3)]
        [DataRow(5, 4)]
        [DataRow(6, 3)]
        [DataRow(6, 4)]
        [DataRow(7, 4)]
        public void TestHorizontalWin(int colNum, int positionWithinFour)
        {
            // Arrange
            TurnModel turn = new TurnModel
            {
                Num = 1, // Player 1
                RowNum = 6,
                ColNum = colNum
            };
            RoomModel room = new RoomModel();
            room.Players.Add(new PlayerModel());
            room.Players.Add(new PlayerModel());
            // Set up board to the left of turn
            for (int c = colNum - (positionWithinFour - 1) - 1; c < colNum - 1; c++)
            {
                room.Board[5, c] = 1;
            }
            // Set up board to the right of turn
            for (int c = colNum + 1 - 1; c <= (colNum - 1) + (4 - positionWithinFour); c++)
            {
                room.Board[5, c] = 1;
            }

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestOpponentPieceDoesntWin()
        {
            // Arrange
            RoomModel room = new RoomModel();
            room.Players.Add(new PlayerModel());
            room.Players.Add(new PlayerModel());
            room.Board[5, 0] = 1;
            room.Board[5, 1] = 1;
            room.Board[5, 2] = 1;
            TurnModel turn = new TurnModel
            {
                Num = 2, // Player 2
                RowNum = 6,
                ColNum = 4
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestFourthPieceUpWins()
        {
            // Arrange
            RoomModel room = new RoomModel();
            room.Players.Add(new PlayerModel());
            room.Players.Add(new PlayerModel());
            room.Board[5, 0] = 1;
            room.Board[4, 0] = 1;
            room.Board[3, 0] = 1;
            TurnModel turn = new TurnModel
            {
                Num = 1, // Player 1
                RowNum = 3,
                ColNum = 1
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
