using System;

using ConnectFour.App;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.App
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        [DataRow(0, 0, 0, 0, "0 Minute")]
        [DataRow(0, 0, 0, 1, "0 Minute")]
        [DataRow(0, 0, 0, 59, "0 Minute")]
        [DataRow(0, 0, 0, 60, "1 Minute")]
        [DataRow(0, 0, 1, 0, "1 Minute")]
        [DataRow(0, 0, 2, 0, "2 Minutes")]
        [DataRow(0, 0, 59, 0, "59 Minutes")]
        [DataRow(0, 0, 59, 59, "59 Minutes")]
        [DataRow(0, 0, 59, 60, "1 Hour")]
        [DataRow(0, 0, 60, 0, "1 Hour")]
        [DataRow(0, 1, 0, 0, "1 Hour")]
        [DataRow(0, 2, 0, 0, "2 Hours")]
        [DataRow(0, 23, 0, 0, "23 Hours")]
        [DataRow(0, 23, 59, 60, "1 Day")]
        [DataRow(0, 24, 0, 0, "1 Day")]
        [DataRow(1, 0, 0, 0, "1 Day")]
        [DataRow(2, 0, 0, 0, "2 Days")]
        [DataRow(10, 20, 30, 40, "10 Days")]
        [DataRow(1111, 2222, 3333, 4444, "1205 Days")]
        public void GetGameDuration_TimeSpan_RendersCorrectly(
            int days,
            int hours,
            int minutes,
            int seconds,
            string expected
        )
        {
            // Arrange
            TimeSpan duration = new TimeSpan(days, hours, minutes, seconds);

            // Act
            string actual = Program.GetGameDuration(duration);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
