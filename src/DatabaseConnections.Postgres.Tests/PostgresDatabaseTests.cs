using Npgsql;
using NUnit.Framework;

namespace DatabaseConnections.Postgres.Tests
{
    [TestFixture]
    [Parallelizable]
    public class PostgresDatabaseTests
    {
        [Test]
        public void Constructor_ShouldNot_ThrowException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new PostgresDatabase(new NpgsqlConnection()));
        }
    }
}
