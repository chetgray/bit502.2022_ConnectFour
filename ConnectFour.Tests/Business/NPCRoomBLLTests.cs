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
