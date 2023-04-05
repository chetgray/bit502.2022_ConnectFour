using System;

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

        // ↗
        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(1, 1)]
        [DataRow(1, 2)]
        [DataRow(1, 3)]
        [DataRow(2, 0)]
        [DataRow(2, 1)]
        [DataRow(2, 2)]
        [DataRow(2, 3)]
        [DataRow(3, 0)]
        [DataRow(3, 1)]
        [DataRow(3, 2)]
        [DataRow(3, 3)]
        [DataRow(4, 0)]
        [DataRow(4, 1)]
        [DataRow(4, 2)]
        [DataRow(4, 3)]
        public void CheckForWin_FourthPieceDiagonalAscending_WillWin(
            int runStartColNum,
            int positionWithinRun
        )
        {
            // Arrange
            const int runStartRowNum = 6;
            TurnModel turn = new TurnModel
            {
                Num = 1,
                RowNum = runStartRowNum - positionWithinRun,
                ColNum = runStartColNum + positionWithinRun
            };
            RoomModel room = new RoomModel();
            // Set up run, skipping the turn cell
            for (
                int r = runStartRowNum - 1, c = runStartColNum - 1;
                c < runStartColNum + 4 - 1;
                r--, c++
            )
            {
                if (r == turn.RowNum - 1 && c == turn.ColNum - 1)
                {
                    continue;
                }
                room.Board[r, c] = 1;
            }

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        // ↘
        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(1, 1)]
        [DataRow(1, 2)]
        [DataRow(1, 3)]
        [DataRow(2, 0)]
        [DataRow(2, 1)]
        [DataRow(2, 2)]
        [DataRow(2, 3)]
        [DataRow(3, 0)]
        [DataRow(3, 1)]
        [DataRow(3, 2)]
        [DataRow(3, 3)]
        [DataRow(4, 0)]
        [DataRow(4, 1)]
        [DataRow(4, 2)]
        [DataRow(4, 3)]
        public void CheckForWin_FourthPieceDiagonalDescending_WillWin(
            int runStartColNum,
            int positionWithinRun
        )
        {
            // Arrange
            const int runStartRowNum = 1;
            TurnModel turn = new TurnModel
            {
                Num = 1,
                RowNum = runStartRowNum + positionWithinRun,
                ColNum = runStartColNum + positionWithinRun
            };
            RoomModel room = new RoomModel();
            // Set up run, skipping the turn cell
            for (
                int r = runStartRowNum - 1, c = runStartColNum - 1;
                c < runStartColNum + 4 - 1;
                r++, c++
            )
            {
                if (r == turn.RowNum - 1 && c == turn.ColNum - 1)
                {
                    continue;
                }
                room.Board[r, c] = 1;
            }

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        // →
        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(1, 1)]
        [DataRow(1, 2)]
        [DataRow(1, 3)]
        [DataRow(2, 0)]
        [DataRow(2, 1)]
        [DataRow(2, 2)]
        [DataRow(2, 3)]
        [DataRow(3, 0)]
        [DataRow(3, 1)]
        [DataRow(3, 2)]
        [DataRow(3, 3)]
        [DataRow(4, 0)]
        [DataRow(4, 1)]
        [DataRow(4, 2)]
        [DataRow(4, 3)]
        public void CheckForWin_FourthPieceHorizontal_WillWin(
            int runStartColNum,
            int positionWithinRun
        )
        {
            // Arrange
            int rowNum = 6 - positionWithinRun;
            TurnModel turn = new TurnModel
            {
                Num = 1,
                RowNum = rowNum,
                ColNum = runStartColNum + positionWithinRun
            };
            RoomModel room = new RoomModel();
            // Set up run, skipping the turn cell
            for (int c = runStartColNum - 1; c < runStartColNum + 4 - 1; c++)
            {
                if (c == turn.ColNum - 1)
                {
                    continue;
                }
                room.Board[rowNum - 1, c] = 1;
            }

            // Act
            bool result = room.CheckForWin(turn);

            // Assert
            Assert.IsTrue(result);
        }

        // ↑
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

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(8)]
        public void GetNextRowInCol_OutOfRange_ThrowsArgumentException(int colNum)
        {
            // Arrange
            RoomModel room = new RoomModel();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(
                () => room.GetNextRowInCol(colNum),
                "Please choose a column between 1 - 7"
            );
        }

        [TestMethod]
        public void GetNextRowInCol_ColumnFull_ThrowsArgumentException()
        {
            // Arrange
            const int colNum = 1;
            RoomModel room = new RoomModel()
            {
                Board = new int[,]
                { //                        index / RowNum
                    { 2, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                    { 1, 0, 0, 0, 0, 0, 0 }, // 1 / 2
                    { 2, 0, 0, 0, 0, 0, 0 }, // 2 / 3
                    { 1, 0, 0, 0, 0, 0, 0 }, // 3 / 4
                    { 2, 0, 0, 0, 0, 0, 0 }, // 4 / 5
                    { 1, 0, 0, 0, 0, 0, 0 }, // 5 / 6
                    //0, 1, 2, 3, 4, 5, 6 - index
                    //1, 2, 3, 4, 5, 6, 7 - ColNum
                }
            };
            for (int rowNum = 1; rowNum <= 6; rowNum++)
            {
                room.Turns.Add(new TurnModel { RowNum = rowNum, ColNum = colNum, });
            }

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(
                () => room.GetNextRowInCol(colNum),
                $"Column {colNum} is Full!"
            );
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        public void GetNextRowInCol_ColumnNotFull_ReturnsNextCol(int fullRowCount)
        {
            // Arrange
            const int colNum = 1;
            RoomModel room = new RoomModel()
            {
                Board = new int[,]
                { //                        index / RowNum
                    { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                    { 0, 0, 0, 0, 0, 0, 0 }, // 1 / 2
                    { 0, 0, 0, 0, 0, 0, 0 }, // 2 / 3
                    { 0, 0, 0, 0, 0, 0, 0 }, // 3 / 4
                    { 0, 0, 0, 0, 0, 0, 0 }, // 4 / 5
                    { 1, 0, 0, 0, 0, 0, 0 }, // 5 / 6
                    //0, 1, 2, 3, 4, 5, 6 - index
                    //1, 2, 3, 4, 5, 6, 7 - ColNum
                }
            };
            for (int rowNum = 1; rowNum <= fullRowCount; rowNum++)
            {
                room.Turns.Add(new TurnModel { RowNum = rowNum, ColNum = colNum, });
            }

            // Act
            int actualResultRowNum = room.GetNextRowInCol(colNum);

            // Assert
            Assert.AreEqual(6 - fullRowCount, actualResultRowNum);
        }
    }
}
