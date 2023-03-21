using System.Data;

using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;
using ConnectFour.Tests.TestDoubles;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.Data
{
    [TestClass]
    public class PlayerRepositoryTests
    {
        [TestMethod]
        public void GetPlayersInRoom_NoPlayers_BothSeatsNull()
        {
            // Arrange
            DALStub dal = new DALStub { TestDataTable = CreatePlayerTable() };
            IPlayerRepository repository = new PlayerRepository(dal);

            // Act
            PlayerDTO[] actual = repository.GetPlayersInRoom(0);

            // Assert
            Assert.AreEqual(2, actual.Length);
            foreach (PlayerDTO dto in actual)
            {
                Assert.IsNull(dto);
            }
        }

        [TestMethod]
        public void GetPlayersInRoom_OnePlayerWithNumOne_SeatOneFullSeatTwoNull()
        {
            // Arrange
            DALStub dal = new DALStub { TestDataTable = CreatePlayerTable() };
            dal.TestDataTable.Rows.Add(1, "TestPlayerOne", 0, 1);
            IPlayerRepository repository = new PlayerRepository(dal);

            // Act
            PlayerDTO[] actual = repository.GetPlayersInRoom(0);

            // Assert
            Assert.AreEqual(2, actual.Length);
            Assert.IsNotNull(actual[0]);
            Assert.IsNull(actual[1]);
        }

        [TestMethod]
        public void GetPlayersInRoom_OnePlayerWithNumTwo_SeatOneNullSeatTwoFull()
        {
            // Arrange
            DALStub dal = new DALStub { TestDataTable = CreatePlayerTable() };
            dal.TestDataTable.Rows.Add(1, "TestPlayerTwo", 0, 2);
            IPlayerRepository repository = new PlayerRepository(dal);

            // Act
            PlayerDTO[] actual = repository.GetPlayersInRoom(0);

            // Assert
            Assert.AreEqual(2, actual.Length);
            Assert.IsNull(actual[0]);
            Assert.IsNotNull(actual[1]);
        }

        [TestMethod]
        public void GetPlayersInRoom_TwoPlayers_TwoSeatsHasPlayers()
        {
            // Arrange
            DALStub dal = new DALStub { TestDataTable = CreatePlayerTable() };
            dal.TestDataTable.Rows.Add(1, "TestPlayerOne", 0, 1);
            dal.TestDataTable.Rows.Add(1, "TestPlayerTwo", 0, 2);
            IPlayerRepository repository = new PlayerRepository(dal);

            // Act
            PlayerDTO[] actual = repository.GetPlayersInRoom(0);

            // Assert
            Assert.AreEqual(2, actual.Length);
            Assert.IsNotNull(actual[0]);
            Assert.IsNotNull(actual[1]);
        }

        private static DataTable CreatePlayerTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("PlayerId", typeof(int));
            table.Columns.Add("PlayerName", typeof(string));
            table.Columns.Add("PlayerRoomId", typeof(int));
            table.Columns.Add("PlayerNum", typeof(int));
            return table;
        }
    }
}
