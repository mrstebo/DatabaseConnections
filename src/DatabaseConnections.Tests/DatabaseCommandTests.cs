using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DatabaseCommandTests
    {
        [Test]
        public void CommandText_ShouldReturn_Null()
        {
            var command = new DatabaseCommand();

            Assert.IsNull(command.CommandText);
        }

        [Test]
        public void Parameters_ShouldReturn_EmptyList()
        {
            var command = new DatabaseCommand();

            Assert.IsNotNull(command.Parameters);
            Assert.AreEqual(0, command.Parameters.Count);
        }
    }
}