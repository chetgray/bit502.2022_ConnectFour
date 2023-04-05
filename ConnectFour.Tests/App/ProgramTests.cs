using System;

using ConnectFour.App;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectFour.Tests.App
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        [DataRow(0, 0, 0, 0, 1, "0s")]
        [DataRow(0, 0, 0, 0, 2, "0s")]
        [DataRow(0, 0, 0, 1, 1, "1s")]
        [DataRow(0, 0, 0, 59, 1, "59s")]
        [DataRow(0, 0, 0, 60, 1, "1m")]
        [DataRow(0, 0, 1, 0, 1, "1m")]
        [DataRow(0, 0, 2, 0, 1, "2m")]
        [DataRow(0, 0, 59, 0, 1, "59m")]
        [DataRow(0, 0, 59, 59, 1, "59m")]
        [DataRow(0, 0, 59, 60, 1, "1h")]
        [DataRow(0, 0, 59, 60, 2, "1h 0m")]
        [DataRow(0, 0, 60, 0, 1, "1h")]
        [DataRow(0, 1, 0, 0, 1, "1h")]
        [DataRow(0, 2, 0, 0, 1, "2h")]
        [DataRow(0, 23, 0, 0, 1, "23h")]
        [DataRow(0, 23, 59, 60, 1, "1d")]
        [DataRow(0, 24, 0, 0, 1, "1d")]
        [DataRow(1, 0, 0, 0, 1, "1d")]
        [DataRow(1, 0, 1, 0, 1, "1d")]
        [DataRow(1, 0, 1, 0, 2, "1d 0h")]
        [DataRow(1, 0, 1, 0, 3, "1d 0h 1m")]
        [DataRow(2, 0, 0, 0, 1, "2d")]
        [DataRow(10, 20, 30, 40, 1, "10d")]
        [DataRow(10, 20, 30, 40, 2, "10d 20h")]
        [DataRow(10, 20, 30, 40, 3, "10d 20h 30m")]
        [DataRow(10, 20, 30, 40, 4, "10d 20h 30m 40s")]
        [DataRow(1111, 2222, 3333, 4444, 1, "1205d")]
        [DataRow(1111, 2222, 3333, 4444, 2, "1205d 22h")]
        [DataRow(1111, 2222, 3333, 4444, 3, "1205d 22h 47m")]
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
