﻿using ConnectFour.Business.BLLs;
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
        [TestMethod]
        public void AddPlayerToRoom_SeatOneOpenSeatTwoFull_NewPlayerTakesSeatOne()
        {
            // Arrange
            const string newPlayerName = "Player One";
            IPlayerModel newPlayer = new PlayerModel { Name = newPlayerName, Num = 1 };
            IPlayerModel[] players = new IPlayerModel[]
            {
                null,
                new PlayerModel { Name = "Player Two", Num = 2 }
            };
            IRoomModel room = new RoomModel { Id = 0, Players = players };
            IRoomRepository repository = new RoomRepositoryStub
            {
                TestDto = new RoomDTO { Id = 0 }
            };
            IPlayerBLL playerBLL = new PlayerBLLStub
            {
                TestModel = newPlayer,
                TestModels = players
            };
            IRoomBLL bll = new RoomBLL(repository, playerBLL);

            // Act
            IRoomModel resultRoom = bll.AddPlayerToRoom(newPlayerName, (int)room.Id);

            // Assert
            Assert.AreEqual(2, resultRoom.Players.Length);
            Assert.AreEqual(newPlayerName, resultRoom.Players[0].Name);
        }

        [TestMethod]
        public void GetRoomById_IdDoesntExist_ReturnNull()
        {
            // Arrange
            IRoomRepository repository = new RoomRepositoryStub { TestDto = null };
            IPlayerBLL playerBLL = new PlayerBLLStub();
            IRoomBLL bll = new RoomBLL(repository, playerBLL);

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
            IRoomBLL bll = new RoomBLL(repository, playerBLL);

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
            IRoomBLL bll = new RoomBLL(repository, playerBLL);

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
            IRoomBLL bll = new RoomBLL(repository, playerBLL);

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
            IRoomBLL bll = new RoomBLL(repository, playerBLL);

            // Act
            IRoomModel result = bll.GetRoomById(0);

            // Assert
            Assert.IsTrue(result.Vacancy);
        }
    }
}
