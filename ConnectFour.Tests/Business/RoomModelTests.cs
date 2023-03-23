using ConnectFour.Business.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.Business
{
    [TestClass]
    public class RoomModelTests
    {
        [TestMethod]
        public void CheckForWin_FourInARowInterruptedByOpponent_WontWin()
        {
            // Arrange
            RoomModel room = new RoomModel()
            {
                Board = new int[,]
                { //                        index / RowNum
                    { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                    { 0, 0, 0, 0, 0, 0, 0 }, // 1 / 2
                    { 0, 0, 0, 0, 0, 0, 0 }, // 2 / 3
                    { 0, 0, 0, 0, 0, 0, 0 }, // 3 / 4
                    { 0, 0, 0, 0, 0, 0, 0 }, // 4 / 5
                    { 1, 2, 1, 0, 1, 2, 1 }, // 5 / 6
                    //0, 1, 2, 3, 4, 5, 6 - index
                    //1, 2, 3, 4, 5, 6, 7 - ColNum
                }
            };
            TurnModel turn = new TurnModel
            {
                Num = 1,
                RowNum = 6,
                ColNum = 4
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsFalse(result);
        }

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
        public void CheckForWin_FourthPieceHorizontal_WillWin(
            int colNum,
            int positionWithinFour
        )
        {
            // Arrange
            int rowNum = positionWithinFour;
            TurnModel turn = new TurnModel
            {
                Num = 1,
                RowNum = rowNum,
                ColNum = colNum
            };
            RoomModel room = new RoomModel();
            // Set up board to the left of turn
            for (int c = colNum - (positionWithinFour - 1) - 1; c < colNum - 1; c++)
            {
                room.Board[rowNum - 1, c] = 1;
            }
            // Set up board to the right of turn
            for (int c = colNum + 1 - 1; c <= (colNum - 1) + (4 - positionWithinFour); c++)
            {
                room.Board[rowNum - 1, c] = 1;
            }

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckForWin_FourthPieceUp_WillWin()
        {
            // Arrange
            RoomModel room = new RoomModel()
            {
                Board = new int[,]
                { //                        index / RowNum
                    { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                    { 0, 0, 0, 0, 0, 0, 0 }, // 1 / 2
                    { 0, 0, 0, 0, 0, 0, 0 }, // 2 / 3
                    { 1, 0, 0, 0, 0, 0, 0 }, // 3 / 4
                    { 1, 0, 0, 0, 0, 0, 0 }, // 4 / 5
                    { 1, 0, 0, 0, 0, 0, 0 }, // 5 / 6
                    //0, 1, 2, 3, 4, 5, 6 - index
                    //1, 2, 3, 4, 5, 6, 7 - ColNum
                }
            };
            TurnModel turn = new TurnModel
            {
                Num = 1,
                RowNum = 3,
                ColNum = 1
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckForWin_PieceCompletesOpponentsFourInARow_WontWin()
        {
            // Arrange
            RoomModel room = new RoomModel()
            {
                Board = new int[,]
                { //                        index / RowNum
                    { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                    { 0, 0, 0, 0, 0, 0, 0 }, // 1 / 2
                    { 0, 0, 0, 0, 0, 0, 0 }, // 2 / 3
                    { 0, 0, 0, 0, 0, 0, 0 }, // 3 / 4
                    { 0, 0, 0, 0, 0, 0, 0 }, // 4 / 5
                    { 2, 2, 2, 0, 0, 0, 0 }, // 5 / 6
                    //0, 1, 2, 3, 4, 5, 6 - index
                    //1, 2, 3, 4, 5, 6, 7 - ColNum
                }
            };
            TurnModel turn = new TurnModel
            {
                Num = 1,
                RowNum = 6,
                ColNum = 4
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(1, 1, 1, 1)]
        [DataRow(4, 3, 2, 1)]
        public void CheckForWin_PlayInOccupiedCell_WontWin(
            int rowNum,
            int colNum,
            int existingPieceNum,
            int newPieceNum
        )
        {
            // Arrange
            RoomModel room = new RoomModel();
            room.Board[rowNum - 1, colNum - 1] = existingPieceNum;
            TurnModel turn = new TurnModel
            {
                Num = newPieceNum,
                RowNum = rowNum,
                ColNum = colNum
            };

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
