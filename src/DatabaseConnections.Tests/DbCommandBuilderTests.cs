using System.Data;
using Moq;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DbCommandBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _connection = new Mock<IDbConnection>();
            _transaction = new Mock<IDbTransaction>();
            _commandBuilder = new DbCommandBuilder();
        }

        private MockRepository _mockRepository;
        private Mock<IDbConnection> _connection;
        private Mock<IDbTransaction> _transaction;
        private IDbCommandBuilder _commandBuilder;

        [Test]
        public void Build_ShouldCreate_DbCommand()
        {
            var command = new DatabaseCommand
            {
                CommandText = "SELECT * FROM [products] WHERE ID=@ID",
                Parameters = new[]
                {
                    new DbParam("@ID", 1)
                }
            };
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);

            var result = _commandBuilder.BuildCommand(command, _connection.Object, _transaction.Object);

            Assert.AreEqual(command.CommandText, result.CommandText);
            Assert.AreEqual(command.Parameters.Count, result.Parameters.Count);

            for (var i = 0; i < command.Parameters.Count; i++)
            {
                var expected = command.Parameters[i];
                var p = result.Parameters[i] as IDataParameter;

                Assert.IsNotNull(p);
                Assert.AreEqual(expected.ParameterName, p.ParameterName);
                Assert.AreEqual(expected.DbType, p.DbType);
                Assert.AreEqual(expected.Value, p.Value);
            }
        }
    }
}