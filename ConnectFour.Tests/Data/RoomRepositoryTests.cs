using System;
using System.Data;

using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;
using ConnectFour.Tests.TestDoubles;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.Data
{
    [TestClass]
    public class RoomRepositoryTests
    {
        [TestMethod]
        public void GetRoomById_DoesntExist_ReturnsNull()
        {
            // Arrange
            DALStub dal = new DALStub { TestDataTable = CreateRoomTable() };
            IRoomRepository repository = new RoomRepository(dal);

            // Act
            RoomDTO result = repository.GetRoomById(0);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRoomById_MultipleResultRows_ReturnsFirstResult()
        {
            // Arrange
            DALStub dal = new DALStub { TestDataTable = CreateRoomTable() };
            DateTime creationTime = DateTime.Now;
            dal.TestDataTable.Rows.Add(0, creationTime, 1, null);
            dal.TestDataTable.Rows.Add(0, creationTime.AddMinutes(-30), 1, 1);
            dal.TestDataTable.Rows.Add(0, creationTime.AddMinutes(-999), 999, -999);
            IRoomRepository repository = new RoomRepository(dal);

            // Act
            RoomDTO actual = repository.GetRoomById(0);

            // Assert
            Assert.AreEqual(0, actual.Id);
            Assert.AreEqual(creationTime, actual.CreationTime);
            Assert.AreEqual(1, actual.CurrentTurnNumber);
            Assert.AreEqual(null, actual.ResultCode);
        }

        [DataRow(3, 2, 1)]
        [DataRow(-1, 1, null)]
        [DataRow(1, 2, null)]
        [DataRow(1, 1, -1)]
        [DataRow(0, 0, 0)]
        [TestMethod]
        public void GetRoomById_RoomExists_ReturnsRoom(
            int id,
            int? currentTurnNumber,
            int? resultCode
        )
        {
            // Arrange
            DALStub dal = new DALStub { TestDataTable = CreateRoomTable() };
            DateTime creationTime = DateTime.Now;
            dal.TestDataTable.Rows.Add(id, creationTime, currentTurnNumber, resultCode);
            IRoomRepository repository = new RoomRepository(dal);

            // Act
            RoomDTO actual = repository.GetRoomById(id);

            // Assert
            Assert.AreEqual(id, actual.Id);
            Assert.AreEqual(creationTime, actual.CreationTime);
            Assert.AreEqual(currentTurnNumber, actual.CurrentTurnNumber);
            Assert.AreEqual(resultCode, actual.ResultCode);
        }

        private static DataTable CreateRoomTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("RoomId", typeof(int));
            table.Columns.Add("RoomCreationTime", typeof(DateTime));
            table.Columns.Add("RoomCurrentTurnNum", typeof(int));
            table.Columns.Add("RoomResultCode", typeof(int));
            return table;
        }
    }
}
