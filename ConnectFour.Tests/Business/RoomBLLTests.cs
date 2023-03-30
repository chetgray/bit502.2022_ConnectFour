using System;
using System.Collections.Generic;
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
    public class RoomBLLTests
    {
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void AddPlayerToRoom_RoomIsFinished_ThrowsArgumentException(int? resultCode)
        {
            // Arrange
            const string newPlayerName = "New Player";
            IRoomRepository repository = new RoomRepositoryStub
            {
                TestDto = new RoomDTO { Id = 1, ResultCode = resultCode }
            };
            IPlayerBLL playerBLL = new PlayerBLLStub { TestModels = new IPlayerModel[2] };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(
                () => bll.AddPlayerToRoom(newPlayerName, 0),
                $"Room Id 0 is already finished!"
            );
        }

        [TestMethod]
        public void AddPlayerToRoom_RoomNotFound_ThrowsArgumentException()
        {
            // Arrange
            const string newPlayerName = "New Player";
            IRoomRepository repository = new RoomRepositoryStub { TestDto = null };
            IPlayerBLL playerBLL = new PlayerBLLStub();
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(
                () => bll.AddPlayerToRoom(newPlayerName, 0),
                $"Room Id 0 does not match any open rooms."
            );
        }

        [TestMethod]
        public void AddPlayerToRoom_SeatOneFullSeatTwoFull_ThrowsArgumentException()
        {
            // Arrange
            const string newPlayerName = "New Player";
            IRoomRepository repository = new RoomRepositoryStub
            {
                TestDto = new RoomDTO { Id = 0 }
            };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModels = (
                    new IPlayerModel[]
                    {
                        new PlayerModel { Name = "Player One", Num = 1 },
                        new PlayerModel { Name = "Player Two", Num = 2 }
                    }
                )
            };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(
                () => bll.AddPlayerToRoom(newPlayerName, 0),
                $"Room Id 0 is full!"
            );
        }

        [TestMethod]
        public void AddPlayerToRoom_BothSeatsOpen_NewPlayerTakesASeat()
        {
            // Arrange
            const string newPlayerName = "New Player";
            IRoomRepository repository = new RoomRepositoryStub
            {
                TestDto = new RoomDTO { Id = 1 }
            };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModel = new PlayerModel { Name = newPlayerName, Num = 2 },
                TestModels = (new IPlayerModel[] { null, null })
            };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act
            IRoomModel resultRoom = bll.AddPlayerToRoom(newPlayerName, 1);

            // Assert
            Assert.AreEqual(2, resultRoom.Players.Length);
            Assert.IsTrue(resultRoom.Vacancy);
            Assert.IsNotNull(resultRoom.Players.Where(p => p.Name == newPlayerName));
        }

        [TestMethod]
        public void AddPlayerToRoom_SeatOneFullSeatTwoOpen_NewPlayerTakesSeatOne()
        {
            // Arrange
            const string newPlayerName = "New Player";
            IRoomRepository repository = new RoomRepositoryStub
            {
                TestDto = new RoomDTO { Id = 0 }
            };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModel = new PlayerModel { Name = newPlayerName, Num = 2 },
                TestModels = (
                    new IPlayerModel[]
                    {
                        new PlayerModel { Name = "Player One", Num = 1 },
                        null
                    }
                )
            };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act
            IRoomModel resultRoom = bll.AddPlayerToRoom(newPlayerName, 0);

            // Assert
            Assert.AreEqual(2, resultRoom.Players.Length);
            Assert.AreEqual(newPlayerName, resultRoom.Players[1].Name);
        }

        [TestMethod]
        public void AddPlayerToRoom_SeatOneOpenSeatTwoFull_NewPlayerTakesSeatOne()
        {
            // Arrange
            const string newPlayerName = "New Player";
            IRoomRepository repository = new RoomRepositoryStub
            {
                TestDto = new RoomDTO { Id = 0 }
            };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModel = new PlayerModel { Name = newPlayerName, Num = 1 },
                TestModels = (
                    new IPlayerModel[]
                    {
                        null,
                        new PlayerModel { Name = "Player Two", Num = 2 }
                    }
                )
            };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act
            IRoomModel resultRoom = bll.AddPlayerToRoom(newPlayerName, 0);

            // Assert
            Assert.AreEqual(2, resultRoom.Players.Length);
            Assert.AreEqual(newPlayerName, resultRoom.Players[0].Name);
        }

        [DataRow(0)]
        [DataRow(8)]
        [TestMethod]
        public void AddTurnToRoom_ReponseIsNotInRange_ReturnRoomWithErrorMessage(
            int ColumnChoice
        )
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub();
            IPlayerBLL playerBLL = new PlayerBLLStub();
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);
            IRoomModel room = new RoomModel
            {
                Id = 0,
                CurrentTurnNum = 2,
                LocalPlayerNum = 2,
                Players = new PlayerModel[]
                {
                    new PlayerModel
                    {
                        Id = 0,
                        Name = "testOne",
                        Num = 1,
                        Color = ConsoleColor.Red,
                        Symbol = "1"
                    },
                    new PlayerModel
                    {
                        Id = 1,
                        Name = "testTwo",
                        Num = 2,
                        Color = ConsoleColor.Yellow,
                        Symbol = "2"
                    }
                }
            };

            // Act
            IRoomModel result = bll.AddTurnToRoom(ColumnChoice, room);
            // Assert
            Assert.AreEqual("Please choose a column between 1 - 7", result.Message);
            Assert.AreEqual(2, result.CurrentTurnNum);
            Assert.AreEqual(2, result.CurrentPlayerNum);
        }

        [TestMethod]
        public void ConvertToResultModel_PlayerNamesTheSame()
        {
            // Arrange
            IPlayerModel[] players = new IPlayerModel[]
            {
                new PlayerModel { Num = 1, Name = "Player One" },
                new PlayerModel { Num = 2, Name = "Player Two" },
            };
            IRoomModel room = new RoomModel
            {
                Id = 1,
                Players = players,
                Turns = new List<ITurnModel> { new TurnModel() },
            };

            // Act
            IResultModel result = RoomBLL.ConvertToResultModel(room);

            // Assert
            Assert.AreEqual(players.Length, result.Players.Length);
            for (int i = 0; i < players.Length; i++)
            {
                Assert.AreEqual(players[i].Name, result.Players[i]);
            }
        }

        public void GetLastTurnInRoom_RoomCurrentTurnNumOne_NewTurnRecievedIsNull()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub();
            IPlayerBLL playerBLL = new PlayerBLLStub();
            DateTime currentTime = DateTime.Now;
            ITurnBLL turnBLL = new TurnBLLStub { TestModel = null };
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);
            IRoomModel room = new RoomModel
            {
                Id = 0,
                CurrentTurnNum = 1,
                LocalPlayerNum = 1,
                Players = new PlayerModel[]
                {
                    new PlayerModel
                    {
                        Id = 0,
                        Name = "testOne",
                        Num = 1,
                        Color = ConsoleColor.Red,
                        Symbol = "1"
                    },
                    new PlayerModel
                    {
                        Id = 1,
                        Name = "testTwo",
                        Num = 2,
                        Color = ConsoleColor.Yellow,
                        Symbol = "2"
                    }
                }
            };

            // Act
            IRoomModel result = bll.UpdateWithLastTurn(room);
            // Assert
            Assert.AreEqual(1, result.CurrentPlayerNum);
        }

        [TestMethod]
        public void GetLastTurnInRoom_CheckBoardForNewTurnAdded_NewTurnAdded()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub();
            IPlayerBLL playerBLL = new PlayerBLLStub();
            DateTime currentTime = DateTime.Now;
            ITurnBLL turnBLL = new TurnBLLStub
            {
                TestModel = new TurnModel
                {
                    Id = 0,
                    Time = currentTime,
                    RowNum = 6,
                    ColNum = 1,
                    Num = 1
                }
            };
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);
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
                        Name = "testOne",
                        Num = 1,
                        Color = ConsoleColor.Red,
                        Symbol = "1"
                    },
                    new PlayerModel
                    {
                        Id = 1,
                        Name = "testTwo",
                        Num = 2,
                        Color = ConsoleColor.Yellow,
                        Symbol = "2"
                    }
                }
            };

            // Act
            IRoomModel result = bll.UpdateWithLastTurn(room);

            // Assert
            Assert.AreEqual(1, result.Board[5, 0]);
        }

        [TestMethod]
        public void GetRoomById_IdDoesntExist_ReturnNull()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub { TestDto = null };
            IPlayerBLL playerBLL = new PlayerBLLStub();
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // ActG
            IRoomModel result = bll.GetRoomById(0);

            // Assert

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRoomById_SeatOneFullSeatTwoFull_VacancyIsFalse()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub { TestDto = new RoomDTO() };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModels = new IPlayerModel[]
                {
                    new PlayerModel { Num = 1 },
                    new PlayerModel { Num = 2 }
                }
            };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act
            IRoomModel result = bll.GetRoomById(0);

            // Assert
            Assert.IsFalse(result.Vacancy);
        }

        [TestMethod]
        public void GetRoomById_SeatOneFullSeatTwoOpen_VacancyIsTrue()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub { TestDto = new RoomDTO() };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModels = new IPlayerModel[]
                {
                    new PlayerModel { Num = 1 },
                    null
                }
            };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act
            IRoomModel result = bll.GetRoomById(0);

            // Assert
            Assert.IsTrue(result.Vacancy);
        }

        [TestMethod]
        public void GetRoomById_SeatOneOpenSeatTwoFull_VacancyIsTrue()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub { TestDto = new RoomDTO() };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModels = new IPlayerModel[]
                {
                    null,
                    new PlayerModel { Num = 2 }
                }
            };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act
            IRoomModel result = bll.GetRoomById(0);

            // Assert
            Assert.IsTrue(result.Vacancy);
        }

        [TestMethod]
        public void GetRoomById_SeatOneOpenSeatTwoOpen_VacancyIsFalse()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub { TestDto = new RoomDTO() };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModels = new IPlayerModel[] { null, null }
            };
            ITurnBLL turnBLL = new TurnBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);

            // Act
            IRoomModel result = bll.GetRoomById(0);

            // Assert
            Assert.IsTrue(result.Vacancy);
        }

        [TestMethod]
        public void LetThemPlay_Player2Waiting_NewTurnAddedToRoomAndPlayer2Turn()
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
            IRoomBLL bll = new RoomBLL(repository, playerBLL, turnBLL);
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
                        Name = "testOne",
                        Num = 1,
                        Color = ConsoleColor.Red,
                        Symbol = "1"
                    },
                    new PlayerModel
                    {
                        Id = 1,
                        Name = "testTwo",
                        Num = 2,
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
