using System;
using System.Collections.Generic;

using ConnectFour.Business.BLLs;
using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.Repositories.Interfaces;
using ConnectFour.Tests.TestDoubles;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.Business
{
    [TestClass]
    public class NPCRoomBLLTests
    {
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [TestMethod]
        public void GetValidPlay_LastOpenColumn_LastOpenSpaceReturned(int colNum)
        {
            // Arrange
            int[,] board = new int[,]
            {
                //                      index / RowNum
                { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                { 2, 1, 2, 1, 2, 1, 2 }, // 1 / 2
                { 2, 1, 2, 1, 2, 1, 2 }, // 2 / 3
                { 1, 2, 1, 2, 1, 2, 1 }, // 3 / 4
                { 2, 1, 2, 1, 2, 1, 2 }, // 4 / 5
                { 2, 1, 2, 1, 2, 1, 2 }, // 5 / 6
                //0, 1, 2, 3, 4, 5, 6 - index
                //1, 2, 3, 4, 5, 6, 7 - ColNum
            };
            for (int i = 1; i <= 7; i++)
            {
                if (i != colNum)
                {
                    board[0, i - 1] = (i % 2) + 1;
                }
            }
            List<ITurnModel> turns = GenerateTurnsFromBoard(board);
            IRoomModel room = new RoomModel() { Turns = turns, Board = board };

            // Act
            IRoomRepository repository = new RoomRepositoryStub();
            IPlayerBLL playerBLL = new PlayerBLLStub();
            ITurnBLL turnBLL = new TurnBLLStub();
            NPCRoomBLL bll = new NPCRoomBLL(repository, playerBLL, turnBLL);
            TurnModel turn = bll.RandyTakesATurn(room);

            // Assert
            Assert.AreEqual(turn.ColNum, colNum);
        }

        [TestMethod]
        public void GetValidPlays_NoPlaysOnBoard_GetsAllPossiblePlays()
        { 
            // Arrange
            IRoomModel room = new RoomModel()
            {
                Board = new int[,]
                { 
                // index / RowNum
                { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                { 0, 0, 0, 0, 0, 0, 0 }, // 1 / 2
                { 0, 0, 0, 0, 0, 0, 0 }, // 2 / 3
                { 0, 0, 0, 0, 0, 0, 0 }, // 3 / 4
                { 0, 0, 0, 0, 0, 0, 0 }, // 4 / 5
                { 0, 0, 0, 0, 0, 0, 0 }, // 5 / 6 
                //0, 1, 2, 3, 4, 5, 6 - index 
                //1, 2, 3, 4, 5, 6, 7 - ColNum
                }
            }; 

            // Act
            IRoomRepository repository = new RoomRepositoryStub(); 
            IPlayerBLL playerBLL = new PlayerBLLStub(); 
            ITurnBLL turnBLL = new TurnBLLStub(); 
            NPCRoomBLL bll = new NPCRoomBLL(repository, playerBLL, turnBLL); 
            List<(int, int)> possiblePlays = bll.GetValidPlays(room);
            
            // Assert
            Assert.AreEqual(7, possiblePlays.Count); 
        }

            [TestMethod]
        public void LetThemPlay_Player1TurnNPC2Waiting_Player1TakesTurnBeforeNPCTakesTurnAfter()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub();
            IPlayerBLL playerBLL = new PlayerBLLStub();
            DateTime currentTime = DateTime.Now;
            ITurnBLL turnBLL = new TurnBLLStub
            {
                TestModel = new TurnModel
                {
                    Id = 1,
                    Time = currentTime,
                    RowNum = 5,
                    ColNum = 1,
                    Num = 2
                }
            };
            IRoomBLL bll = new NPCRoomBLL(repository, playerBLL, turnBLL);
            IRoomModel room = new RoomModel
            {
                Id = 0,
                CurrentTurnNum = 2,
                LocalPlayerNum = 1,
                Turns =
                        {
                            new TurnModel
                            {
                                Id = 1,
                                Time = currentTime,
                                RowNum = 6,
                                ColNum = 1,
                                Num = 1
                            }
                        },
                Players = new PlayerModel[]
                {
                            new PlayerModel
                            {
                                Id = 0,
                                Name = "testPlayer",
                                Num = 1,
                                Color = ConsoleColor.Red,
                                Symbol = "1"
                            },
                            new PlayerModel
                            {
                                Id = 1,
                                Name = "testNPC",
                                Num = 2,
                                Color = ConsoleColor.Yellow,
                                Symbol = "2"
                            }
                }
            };
        
            // Act
            IRoomModel result = bll.LetThemPlay(room);
        
            // Assert
            Assert.AreEqual(1, result.CurrentPlayerNum);
        }
        
        [TestMethod]
        public void LetThemPlay_NPC1TurnPlayer2Waiting_NPCTakesTurnPlayer1CurrentTurn()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub();
            IPlayerBLL playerBLL = new PlayerBLLStub();
            DateTime currentTime = DateTime.Now;
            ITurnBLL turnBLL = new TurnBLLStub
            {
                TestModel = new TurnModel
                {
                    Id = 1,
                    Time = currentTime,
                    RowNum = 6,
                    ColNum = 1,
                    Num = 1
                }
            };
            IRoomBLL bll = new NPCRoomBLL(repository, playerBLL, turnBLL);
            IRoomModel room = new RoomModel
            {
                Id = 0,
                CurrentTurnNum = 1,
                LocalPlayerNum = 2,
                Players = new PlayerModel[]
                {
                            new PlayerModel
                            {
                                Id = 0,
                                Name = "testPlayer",
                                Num = 2,
                                Color = ConsoleColor.Red,
                                Symbol = "1"
                            },
                            new PlayerModel
                            {
                                Id = 1,
                                Name = "testNPC",
                                Num = 1,
                                Color = ConsoleColor.Yellow,
                                Symbol = "2"
                            }
                }
            };
        
            // Act
            IRoomModel result = bll.LetThemPlay(room);
        
            // Assert
            Assert.AreEqual(2, result.CurrentPlayerNum);
        }

        [TestMethod]
        public void OpheliaTakesATurn_BlockingPlayAvailable_ReturnsBlockingTurn()
        {
            // Arrange
            int[,] board = new int[,]
            {
                //                      index / RowNum
                { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                { 0, 0, 0, 0, 0, 0, 0 }, // 1 / 2
                { 0, 0, 0, 0, 0, 0, 0 }, // 2 / 3
                { 0, 0, 0, 0, 0, 0, 0 }, // 3 / 4
                { 2, 2, 0, 0, 0, 0, 0 }, // 4 / 5
                { 1, 1, 1, 0, 0, 0, 0 }, // 5 / 6
                //0, 1, 2, 3, 4, 5, 6 - index
                //1, 2, 3, 4, 5, 6, 7 - ColNum
            };
            List<ITurnModel> turns = GenerateTurnsFromBoard(board);
            IRoomModel room = new RoomModel
            {
                CurrentTurnNum = 6,
                Turns = turns,
                Board = board,
            };
            IRoomRepository repository = new RoomRepositoryStub();
            IPlayerBLL playerBLL = new PlayerBLLStub();
            ITurnBLL turnBLL = new TurnBLLStub();
            NPCRoomBLL bll = new NPCRoomBLL(repository, playerBLL, turnBLL);
            TurnModel expected = new TurnModel { RowNum = 6, ColNum = 4 };

            // Act
            TurnModel actual = bll.OpheliaTakesATurn(room);

            // Assert
            Assert.AreEqual(expected.RowNum, actual.RowNum);
            Assert.AreEqual(expected.ColNum, actual.ColNum);
        }

        [TestMethod]
        public void OpheliaTakesATurn_WinningPlayAvailable_ReturnsWinningTurn()
        {
            // Arrange
            int[,] board = new int[,]
            {
                //                      index / RowNum
                { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                { 0, 0, 0, 0, 0, 0, 0 }, // 1 / 2
                { 0, 0, 0, 0, 0, 0, 0 }, // 2 / 3
                { 1, 0, 0, 0, 0, 0, 0 }, // 3 / 4
                { 1, 0, 0, 0, 0, 0, 0 }, // 4 / 5
                { 1, 2, 0, 2, 0, 0, 2 }, // 5 / 6
                //0, 1, 2, 3, 4, 5, 6 - index
                //1, 2, 3, 4, 5, 6, 7 - ColNum
            };
            List<ITurnModel> turns = GenerateTurnsFromBoard(board);
            IRoomModel room = new RoomModel
            {
                CurrentTurnNum = 7,
                Turns = turns,
                Board = board,
            };
            IRoomRepository repository = new RoomRepositoryStub();
            IPlayerBLL playerBLL = new PlayerBLLStub();
            ITurnBLL turnBLL = new TurnBLLStub();
            NPCRoomBLL bll = new NPCRoomBLL(repository, playerBLL, turnBLL);
            TurnModel expected = new TurnModel { RowNum = 3, ColNum = 1 };

            // Act
            TurnModel actual = bll.OpheliaTakesATurn(room);

            // Assert
            Assert.AreEqual(expected.RowNum, actual.RowNum);
            Assert.AreEqual(expected.ColNum, actual.ColNum);
        }

        private static List<ITurnModel> GenerateTurnsFromBoard(int[,] board)
        {
            List<ITurnModel> turns = new List<ITurnModel>();
            for (int r = 0; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1); c++)
                {
                    if (board[r, c] != 0)
                    {
                        turns.Add(
                            new TurnModel
                            {
                                ColNum = c + 1,
                                RowNum = r + 1,
                                Num = board[r, c]
                            }
                        );
                    }
                }
            }
            return turns;
        }
    }
}
