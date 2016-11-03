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
            _database = new Mock<IDatabase>();
        }

        private Mock<IDatabase> _database;

        [Test]
        public void ExecuteNonQuery_ShouldCall_Database_ExecuteNonQuery_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.Object.ExecuteNonQuery(commandText, parameters);

            _database.Verify(x => x.ExecuteNonQuery(
                    It.Is<DatabaseCommand>(y =>
                        (y.CommandText == commandText) &&
                        (y.Parameters == parameters))),
                Times.Once);
        }

        [Test]
        public void ExecuteQuery_ShouldCall_Database_ExecuteQuery_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.Object.ExecuteQuery(commandText, parameters);

            _database.Verify(x => x.ExecuteQuery(
                    It.Is<DatabaseCommand>(y =>
                        (y.CommandText == commandText) &&
                        (y.Parameters == parameters))),
                Times.Once);
        }

        [Test]
        public void ExecuteQuery_With_Pagination_ShouldCall_Database_ExecutePagedQuery_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.Object.ExecuteQuery(1, 1, "test", commandText, parameters);

            _database.Verify(x => x.ExecuteQuery(
                    It.Is<DatabaseCommand>(y =>
                        (y.CommandText == commandText) &&
                        (y.Parameters == parameters)),
                    1, 1, "test"),
                Times.Once);
        }

        [Test]
        public void ExecuteReader_ShouldCall_Database_ExecuteReader_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.Object.ExecuteReader(commandText, parameters);

            _database.Verify(x => x.ExecuteReader(
                    It.Is<DatabaseCommand>(y =>
                        (y.CommandText == commandText) &&
                        (y.Parameters == parameters))),
                Times.Once);
        }

        [Test]
        public void ExecuteScalar_ShouldCall_Database_ExecuteScalar_With_DatabaseCommand()
        {
            const string commandText = "COMMAND TEXT";
            var parameters = new[]
            {
                new DbParam("@test", 1)
            };

            _database.Object.ExecuteScalar(commandText, parameters);

            _database.Verify(x => x.ExecuteScalar(
                    It.Is<DatabaseCommand>(y =>
                        (y.CommandText == commandText) &&
                        (y.Parameters == parameters))),
                Times.Once);
        }
    }
}