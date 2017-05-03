using System.Data.OleDb;
using NUnit.Framework;

namespace DatabaseConnections.OleDb.Tests
{
    [TestFixture]
    [Parallelizable]
    public class OleDbDatabaseTests
    {
        [Test]
        public void Constructor_ShouldNot_ThrowException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new OleDbDatabase(new OleDbConnection()));
        }
    }
}
