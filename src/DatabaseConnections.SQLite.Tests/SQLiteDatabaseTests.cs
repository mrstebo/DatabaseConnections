using System.Data.SQLite;
using NUnit.Framework;

namespace DatabaseConnections.SQLite.Tests
{
    [TestFixture]
    [Parallelizable]
    public class SQLiteDatabaseTests
    {
        [Test]
        public void Constructor_ShouldNot_ThrowException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new SQLiteDatabase(new SQLiteConnection()));
        }
    }
}
