using System;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DatabaseCommandExecutedEventArgsTests
    {
        [Test]
        public void Constructor_ShouldSet_Properties()
        {
            var command = new DatabaseCommand();
            var timeTaken = new TimeSpan(1, 2, 3, 4);

            var e = new DatabaseCommandExecutedEventArgs(command, timeTaken);

            Assert.AreEqual(command, e.Command);
            Assert.AreEqual(timeTaken, e.TimeTaken);
        }
    }
}