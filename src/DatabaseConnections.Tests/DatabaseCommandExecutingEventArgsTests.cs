using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DatabaseCommandExecutingEventArgsTests
    {
        [Test]
        public void Constructor_ShouldSet_Properties()
        {
            var command = new DatabaseCommand();

            var e = new DatabaseCommandExecutingEventArgs(command);

            Assert.AreEqual(command, e.Command);
        }
    }
}