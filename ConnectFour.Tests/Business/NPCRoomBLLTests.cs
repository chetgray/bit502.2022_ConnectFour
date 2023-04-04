using System;
using System.Linq;

using ConnectFour.Business.BLLs;
using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
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
            IRoomModel room = new RoomModel()
            {
                Board = new int[,]
                { //                        index / RowNum
                    { 0, 0, 0, 0, 0, 0, 0 }, // 0 / 1
                    { 2, 1, 2, 1, 2, 1, 2 }, // 1 / 2
                    { 2, 1, 2, 1, 2, 1, 2 }, // 2 / 3
                    { 1, 2, 1, 2, 1, 2, 1 }, // 3 / 4
                    { 2, 1, 2, 1, 2, 1, 2 }, // 4 / 5
                    { 2, 1, 2, 1, 2, 1, 2 }, // 5 / 6
                    //0, 1, 2, 3, 4, 5, 6 - index
                    //1, 2, 3, 4, 5, 6, 7 - ColNum
                }
            };

            for (int i = 1; i <= 7; i++)
            {
                if (i != colNum)
                {
                    room.Board[0, i - 1] = (i % 2) + 1;
                }
            }

            for (int row = 0; row < room.Board.GetLength(0); row++)
            {
                for (int column = 0; column < room.Board.GetLength(1); column++)
                {
                    if (room.Board[row, column] != 0)
                    {
                        room.Turns.Add(new TurnModel
                        {
                            Id = null,
                            ColNum = column + 1,
                            RowNum = row + 1,
                            Num = room.Board[row, column]
                        });
                    }
                }
            }

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
    }
}