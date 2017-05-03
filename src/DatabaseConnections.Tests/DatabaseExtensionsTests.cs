using DatabaseConnections.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DatabaseExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
            _connection = new Mock<IDbConnectionWrapper>();
            _database = new DatabaseStub(_connection.Object);
        }

        private Mock<IDbConnectionWrapper> _connection;
        private Database _database;

        [Test]
        public void ExecuteNonQuery_ShouldCall_ExecuteNonQuery_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.ExecuteNonQuery(commandText, parameters);

            _connection.Verify(x => x.ExecuteNonQuery(
                    It.Is<DatabaseCommand>(y =>
                        y.CommandText == commandText &&
                        y.Parameters == parameters)),
                Times.Once);
        }

        [Test]
        public void ExecuteQuery_ShouldCall_ExecuteQuery_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.ExecuteQuery(commandText, parameters);

            _connection.Verify(x => x.ExecuteQuery(
                    It.Is<DatabaseCommand>(y =>
                        y.CommandText == commandText &&
                        y.Parameters == parameters)),
                Times.Once);
        }

        [Test]
        public void ExecutePagedQuery_ShouldCall_ExecuteQuery_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.ExecutePagedQuery(1, 1, "test", commandText, parameters);

            _connection.Verify(x => x.ExecuteQuery(
                    It.Is<DatabaseCommand>(y =>
                        y.CommandText == commandText &&
                        y.Parameters == parameters),
                    1, 1, "test"),
                Times.Once);
        }

        [Test]
        public void ExecuteReader_ShouldCall_ExecuteReader_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.ExecuteReader(commandText, parameters);

            _connection.Verify(x => x.ExecuteReader(
                    It.Is<DatabaseCommand>(y =>
                        y.CommandText == commandText &&
                        y.Parameters == parameters)),
                Times.Once);
        }

        [Test]
        public void ExecuteScalar_ShouldCall_ExecuteScalar_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.ExecuteScalar(commandText, parameters);

            _connection.Verify(x => x.ExecuteScalar(
                    It.Is<DatabaseCommand>(y =>
                        y.CommandText == commandText &&
                        y.Parameters == parameters)),
                Times.Once);
        }
    }
}