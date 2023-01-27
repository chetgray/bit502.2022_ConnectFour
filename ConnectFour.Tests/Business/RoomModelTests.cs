using ConnectFour.Business.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.Business
{
    [TestClass]
    public class RoomModelTests
    {
        [TestMethod]
        public void TestFourthPieceToRightWins()
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
                Num = 1, // Player 1
                RowNum = 6,
                ColNum = 4
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFourthPieceToLeftWins()
        {
            // Arrange
            RoomModel room = new RoomModel();
            room.Players.Add(new PlayerModel());
            room.Players.Add(new PlayerModel());
            room.Board[5, 1] = 1;
            room.Board[5, 2] = 1;
            room.Board[5, 3] = 1;
            TurnModel turn = new TurnModel
            {
                Num = 1, // Player 1
                RowNum = 6,
                ColNum = 1
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMiddleLeftPieceWins()
        {
            // Arrange
            RoomModel room = new RoomModel();
            room.Players.Add(new PlayerModel());
            room.Players.Add(new PlayerModel());
            room.Board[5, 0] = 1;
            room.Board[5, 2] = 1;
            room.Board[5, 3] = 1;
            TurnModel turn = new TurnModel
            {
                Num = 1, // Player 1
                RowNum = 6,
                ColNum = 2
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMiddleRightPieceWins()
        {
            // Arrange
            RoomModel room = new RoomModel();
            room.Players.Add(new PlayerModel());
            room.Players.Add(new PlayerModel());
            room.Board[5, 0] = 1;
            room.Board[5, 1] = 1;
            room.Board[5, 3] = 1;
            TurnModel turn = new TurnModel
            {
                Num = 1, // Player 1
                RowNum = 6,
                ColNum = 3
            };

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
