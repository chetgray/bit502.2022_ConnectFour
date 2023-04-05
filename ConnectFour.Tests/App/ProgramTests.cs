using System;

using ConnectFour.App;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.App
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        [DataRow(0, 0, 0, 0, 1, "0 Minutes")]
        [DataRow(0, 0, 0, 0, 2, "0 Minutes")]
        [DataRow(0, 0, 0, 1, 1, "0 Minutes")]
        [DataRow(0, 0, 0, 59, 1, "0 Minutes")]
        [DataRow(0, 0, 0, 60, 1, "1 Minute")]
        [DataRow(0, 0, 1, 0, 1, "1 Minute")]
        [DataRow(0, 0, 2, 0, 1, "2 Minutes")]
        [DataRow(0, 0, 59, 0, 1, "59 Minutes")]
        [DataRow(0, 0, 59, 59, 1, "59 Minutes")]
        [DataRow(0, 0, 59, 60, 1, "1 Hour")]
        [DataRow(0, 0, 59, 60, 2, "1 Hour 0 Minutes")]
        [DataRow(0, 0, 60, 0, 1, "1 Hour")]
        [DataRow(0, 1, 0, 0, 1, "1 Hour")]
        [DataRow(0, 2, 0, 0, 1, "2 Hours")]
        [DataRow(0, 23, 0, 0, 1, "23 Hours")]
        [DataRow(0, 23, 59, 60, 1, "1 Day")]
        [DataRow(0, 24, 0, 0, 1, "1 Day")]
        [DataRow(1, 0, 0, 0, 1, "1 Day")]
        [DataRow(1, 0, 1, 0, 1, "1 Day")]
        [DataRow(1, 0, 1, 0, 2, "1 Day 0 Hours")]
        [DataRow(1, 0, 1, 0, 3, "1 Day 0 Hours 1 Minute")]
        [DataRow(2, 0, 0, 0, 1, "2 Days")]
        [DataRow(10, 20, 30, 40, 1, "10 Days")]
        [DataRow(10, 20, 30, 40, 2, "10 Days 20 Hours")]
        [DataRow(10, 20, 30, 40, 3, "10 Days 20 Hours 30 Minutes")]
        [DataRow(1111, 2222, 3333, 4444, 1, "1205 Days")]
        [DataRow(1111, 2222, 3333, 4444, 2, "1205 Days 22 Hours")]
        [DataRow(1111, 2222, 3333, 4444, 3, "1205 Days 22 Hours 47 Minutes")]
        public void ToTruncatedString_TimeSpan_RendersCorrectly(
            int days,
            int hours,
            int minutes,
            int seconds,
            int unitCount,
            string expected
        )
        {
            // Arrange
            TimeSpan duration = new TimeSpan(days, hours, minutes, seconds);

            // Act
            string actual = duration.ToTruncatedString(unitCount);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToTruncatedString_UnitCountLessThanOne_ThrowsException()
        {
            // Arrange
            TimeSpan duration = TimeSpan.Zero;
            Action action = () => duration.ToTruncatedString(0);

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(action);
        }
    }
}
