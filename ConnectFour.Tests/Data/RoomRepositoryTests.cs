using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        // ConvertTableToResultDtos
        [TestMethod]
        public void ConvertTableToResultDtos_TableHasNullsInTurnColumns_DtoHasNullsInTurnFields()
        {
            // Arrange
            DataTable table = CreateResultTable();
            table.Rows.Add(1, DateTime.Now, 1, "Player One", 1, DBNull.Value, DBNull.Value);
            table.Rows.Add(1, DateTime.Now, 1, "Player Two", 2, DBNull.Value, DBNull.Value);

            // Act
            IList<ResultDTO> resultDtos = RoomRepository
                .ConvertTableToResultDtos(table)
                .ToList();

            // Assert
            Assert.IsNull(resultDtos[0].LastTurnTime);
            Assert.IsNull(resultDtos[0].LastTurnNum);
        }

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

        private static DataTable CreateResultTable()
        {
            DataTable results = new DataTable();
            results.Columns.Add("RoomId", typeof(int));
            results.Columns.Add("RoomCreationTime", typeof(DateTime));
            results.Columns.Add("RoomResultCode", typeof(int));
            results.Columns.Add("PlayerName", typeof(string));
            results.Columns.Add("PlayerNum", typeof(int));
            results.Columns.Add("TurnTime", typeof(DateTime));
            results.Columns.Add("TurnNum", typeof(int));
            return results;
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
