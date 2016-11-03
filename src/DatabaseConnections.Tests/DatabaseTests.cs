using System;
using System.Data;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    internal class TestDatabase : Database
    {
        public TestDatabase(IDbConnection connection, IDataPopulator dataPopulator, IDbCommandBuilder commandBuilder)
            : base(connection, dataPopulator, commandBuilder)
        {
        }
    }

    [TestFixture]
    [Parallelizable]
    public class DatabaseTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _connection = new Mock<IDbConnection>();
            _dataPopulator = new Mock<IDataPopulator>();
            _commandBuilder = new Mock<IDbCommandBuilder>();
            _database = new TestDatabase(
                _connection.Object,
                _dataPopulator.Object,
                _commandBuilder.Object);
        }

        private MockRepository _mockRepository;
        private Mock<IDbConnection> _connection;
        private Mock<IDataPopulator> _dataPopulator;
        private Mock<IDbCommandBuilder> _commandBuilder;
        private IDatabase _database;

        [Test]
        public void ConnectionString_ShouldReturn_ConnectionString()
        {
            _connection
                .SetupGet(x => x.ConnectionString)
                .Returns("TEST");

            Assert.AreEqual(_database.ConnectionString, "TEST");
        }

        [Test]
        public void ExecuteNonQueries_ShouldCall_Inteceptors_And_ModifyDatabaseCommand()
        {
            var commands = Enumerable.Range(1, 5)
                .Select(x => new DatabaseCommand())
                .ToArray();
            var transaction = new Mock<IDbTransaction>();
            var com = _mockRepository.CreateIDbCommand();
            var interceptor = new Mock<IDatabaseCommandInteceptor>();

            _connection
                .Setup(x => x.BeginTransaction(IsolationLevel.ReadCommitted))
                .Returns(transaction.Object);
            _commandBuilder
                .Setup(x => x.BuildCommand(
                    It.Is<DatabaseCommand>(y => commands.Contains(y)),
                    _connection.Object,
                    transaction.Object))
                .Returns(com.Object);
            interceptor
                .Setup(x => x.Intercept(_database, It.Is<DatabaseCommand>(y => commands.Contains(y))))
                .Callback((IDatabase db, DatabaseCommand command2) => command2.CommandText = "Test");

            _database.Inteceptors.Add(interceptor.Object);

            _database.ExecuteNonQueries(commands);

            for (var i = 0; i < commands.Length; i++)
            {
                var index = i;
                interceptor.Verify(x => x.Intercept(_database, commands[index]));

                Assert.AreEqual("Test", commands[index].CommandText);
            }
        }

        [Test]
        public void ExecuteNonQueries_ShouldInvoke_Executed_Event()
        {
            var invoked = false;
            var command = new DatabaseCommand();
            var transaction = new Mock<IDbTransaction>();
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.BeginTransaction(IsolationLevel.ReadCommitted))
                .Returns(transaction.Object);
            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, transaction.Object))
                .Returns(com.Object);

            _database.Executed += (o, e) => invoked = true;

            _database.ExecuteNonQueries(new[] {command});

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteNonQueries_ShouldInvoke_Executing_Event()
        {
            var invoked = false;
            var command = new DatabaseCommand();
            var transaction = new Mock<IDbTransaction>();
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.BeginTransaction(IsolationLevel.ReadCommitted))
                .Returns(transaction.Object);
            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, transaction.Object))
                .Returns(com.Object);

            _database.Executing += (o, e) => invoked = true;

            _database.ExecuteNonQueries(new[] {command});

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteNonQueries_ShouldReturn_RowsAffected()
        {
            var commands = Enumerable.Range(1, 5)
                .Select(x => new DatabaseCommand())
                .ToArray();
            var transaction = new Mock<IDbTransaction>();
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.BeginTransaction(IsolationLevel.ReadCommitted))
                .Returns(transaction.Object);
            _commandBuilder
                .Setup(x => x.BuildCommand(
                    It.Is<DatabaseCommand>(y => commands.Contains(y)),
                    _connection.Object,
                    transaction.Object))
                .Returns(com.Object);
            com
                .Setup(x => x.ExecuteNonQuery())
                .Returns(1);

            var result = _database.ExecuteNonQueries(commands);

            Assert.AreEqual(5, result);
        }

        [Test]
        public void ExecuteNonQueries_When_DbCommandThrowsException_ShouldThrow_DbCommandException()
        {
            var commands = Enumerable.Range(1, 5)
                .Select(x => new DatabaseCommand())
                .ToArray();
            var transaction = new Mock<IDbTransaction>();
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.BeginTransaction(IsolationLevel.ReadCommitted))
                .Returns(transaction.Object);
            _commandBuilder
                .Setup(x => x.BuildCommand(
                    It.Is<DatabaseCommand>(y => commands.Contains(y)),
                    _connection.Object,
                    transaction.Object))
                .Returns(com.Object);
            com
                .Setup(x => x.ExecuteNonQuery())
                .Throws(new Exception("Error"));

            var ex = Assert.Throws<DatabaseCommandException>(() => _database.ExecuteNonQueries(commands));

            Assert.AreEqual("Failed to execute non queries", ex.Message);
            Assert.AreEqual(commands[0], ex.Command);
        }

        [Test]
        public void ExecuteNonQueries_With_NoCommands_ShouldNot_ThrowError()
        {
            var transaction = new Mock<IDbTransaction>();

            _connection
                .Setup(x => x.BeginTransaction(IsolationLevel.ReadCommitted))
                .Returns(transaction.Object);

            Assert.DoesNotThrow(() => _database.ExecuteNonQueries(null));
        }

        [Test]
        public void ExecuteNonQuery_ShouldCall_Inteceptors_And_ModifyDatabaseCommand()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();
            var interceptor = new Mock<IDatabaseCommandInteceptor>();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            interceptor
                .Setup(x => x.Intercept(_database, command))
                .Callback((IDatabase db, DatabaseCommand command2) => command2.CommandText = "Test");

            _database.Inteceptors.Add(interceptor.Object);

            _database.ExecuteNonQuery(command);

            interceptor.Verify(x => x.Intercept(_database, command));

            Assert.AreEqual("Test", command.CommandText);
        }

        [Test]
        public void ExecuteNonQuery_ShouldInvoke_Executed_Event()
        {
            var invoked = false;
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);

            _database.Executed += (o, e) => invoked = true;

            _database.ExecuteNonQuery(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteNonQuery_ShouldInvoke_Executing_Event()
        {
            var invoked = false;
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);

            _database.Executing += (o, e) => invoked = true;

            _database.ExecuteNonQuery(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteNonQuery_ShouldReturn_RowsAffected()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            com
                .Setup(x => x.ExecuteNonQuery())
                .Returns(1);

            var result = _database.ExecuteNonQuery(command);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ExecuteNonQuery_When_DbCommandThrowsException_ShouldThrow_DbCommandException()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);
            com
                .Setup(x => x.ExecuteNonQuery())
                .Throws(new Exception("Error"));

            var ex = Assert.Throws<DatabaseCommandException>(() => _database.ExecuteNonQuery(command));

            Assert.AreEqual("Failed to execute non query", ex.Message);
            Assert.AreEqual(command, ex.Command);
        }

        [Test]
        public void ExecuteQuery_ShouldCall_Inteceptors_And_ModifyDatabaseCommand()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();
            var interceptor = new Mock<IDatabaseCommandInteceptor>();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);
            interceptor
                .Setup(x => x.Intercept(_database, command))
                .Callback((IDatabase db, DatabaseCommand command2) => command2.CommandText = "Test");

            _database.Inteceptors.Add(interceptor.Object);

            _database.ExecuteQuery(command);

            interceptor.Verify(x => x.Intercept(_database, command));

            Assert.AreEqual("Test", command.CommandText);
        }

        [Test]
        public void ExecuteQuery_ShouldInvoke_Executed_Event()
        {
            var invoked = false;
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);

            _database.Executed += (o, e) => invoked = true;

            _database.ExecuteQuery(new DatabaseCommand());

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteQuery_ShouldInvoke_Executing_Event()
        {
            var invoked = false;
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);

            _database.Executing += (o, e) => invoked = true;

            _database.ExecuteQuery(new DatabaseCommand());

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteQuery_ShouldReturn_Dataset()
        {
            var command = new DatabaseCommand();
            var ds = new DataSet();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            _dataPopulator
                .Setup(x => x.Populate(com.Object))
                .Returns(ds);

            var result = _database.ExecuteQuery(command);

            Assert.AreEqual(ds, result);
        }

        [Test]
        public void ExecuteQuery_When_DbDataAdapterThrowsException_ShouldThrow_DbCommandException()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            _dataPopulator
                .Setup(x => x.Populate(com.Object))
                .Throws(new Exception("Error"));

            var ex = Assert.Throws<DatabaseCommandException>(() => _database.ExecuteQuery(command));

            Assert.AreEqual("Failed to execute query", ex.Message);
            Assert.AreEqual(command, ex.Command);
        }

        [Test]
        public void ExecuteQuery_With_Pagination_ShouldCall_Inteceptors_And_ModifyDatabaseCommand()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();
            var interceptor = new Mock<IDatabaseCommandInteceptor>();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);
            interceptor
                .Setup(x => x.Intercept(_database, command))
                .Callback((IDatabase db, DatabaseCommand command2) => command2.CommandText = "Test");

            _database.Inteceptors.Add(interceptor.Object);

            _database.ExecuteQuery(command, 1, 1, "test");

            interceptor.Verify(x => x.Intercept(_database, command));

            Assert.AreEqual("Test", command.CommandText);
        }

        [Test]
        public void ExecuteQuery_With_Pagination_ShouldInvoke_Executed_Event()
        {
            var invoked = false;
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);

            _database.Executed += (o, e) => invoked = true;

            _database.ExecuteQuery(new DatabaseCommand(), 1, 1, "test");

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteQuery_With_Pagination_ShouldInvoke_Executing_Event()
        {
            var invoked = false;
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);

            _database.Executing += (o, e) => invoked = true;

            _database.ExecuteQuery(new DatabaseCommand(), 1, 1, "test");

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteQuery_With_Pagination_ShouldReturn_Dataset()
        {
            var command = new DatabaseCommand();
            var ds = new DataSet();
            var com = _mockRepository.CreateIDbCommand();


            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            _dataPopulator
                .Setup(x => x.Populate(com.Object, 1, 1, "test"))
                .Returns(ds);

            var result = _database.ExecuteQuery(command, 1, 1, "test");

            Assert.AreEqual(ds, result);
        }

        [Test]
        public void ExecuteQuery_With_Pagination_When_DbCommandThrowsException_ShouldThrow_DbCommandException()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            _dataPopulator
                .Setup(x => x.Populate(com.Object, 1, 1, "test"))
                .Throws(new Exception("Error"));

            var ex = Assert.Throws<DatabaseCommandException>(() => _database.ExecuteQuery(command, 1, 1, "test"));

            Assert.AreEqual("Failed to execute paged query", ex.Message);
            Assert.AreEqual(command, ex.Command);
        }
        
        [Test]
        public void ExecuteReader_ShouldCall_Inteceptors_And_ModifyDatabaseCommand()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();
            var interceptor = new Mock<IDatabaseCommandInteceptor>();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            interceptor
                .Setup(x => x.Intercept(_database, command))
                .Callback((IDatabase db, DatabaseCommand command2) => command2.CommandText = "Test");

            _database.Inteceptors.Add(interceptor.Object);

            _database.ExecuteReader(command);

            interceptor.Verify(x => x.Intercept(_database, command));

            Assert.AreEqual("Test", command.CommandText);
        }

        [Test]
        public void ExecuteReader_ShouldInvoke_Executed_Event()
        {
            var invoked = false;
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);

            _database.Executed += (o, e) => invoked = true;

            _database.ExecuteReader(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteReader_ShouldInvoke_Executing_Event()
        {
            var invoked = false;
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);

            _database.Executing += (o, e) => invoked = true;

            _database.ExecuteReader(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteReader_ShouldReturn_Data()
        {
            var command = new DatabaseCommand();
            var reader = new DataTableReader(new DataTable());
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            com
                .Setup(x => x.ExecuteReader())
                .Returns(reader);

            var result = _database.ExecuteReader(command);

            Assert.AreEqual(reader, result);
        }

        [Test]
        public void ExecuteReader_When_DbCommandThrowsException_ShouldThrow_DbCommandException()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);
            com
                .Setup(x => x.ExecuteReader())
                .Throws(new Exception("Error"));

            var ex = Assert.Throws<DatabaseCommandException>(() => _database.ExecuteReader(command));

            Assert.AreEqual("Failed to execute reader", ex.Message);
            Assert.AreEqual(command, ex.Command);
        }

        [Test]
        public void ExecuteScalar_ShouldCall_Inteceptors_And_ModifyDatabaseCommand()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();
            var interceptor = new Mock<IDatabaseCommandInteceptor>();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            interceptor
                .Setup(x => x.Intercept(_database, command))
                .Callback((IDatabase db, DatabaseCommand command2) => command2.CommandText = "Test");

            _database.Inteceptors.Add(interceptor.Object);

            _database.ExecuteScalar(command);

            interceptor.Verify(x => x.Intercept(_database, command));

            Assert.AreEqual("Test", command.CommandText);
        }

        [Test]
        public void ExecuteScalar_ShouldInvoke_Executed_Event()
        {
            var invoked = false;
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);

            _database.Executed += (o, e) => invoked = true;

            _database.ExecuteScalar(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteScalar_ShouldInvoke_Executing_Event()
        {
            var invoked = false;
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);

            _database.Executing += (o, e) => invoked = true;

            _database.ExecuteScalar(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteScalar_ShouldReturn_ScalarValue()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _commandBuilder
                .Setup(x => x.BuildCommand(command, _connection.Object, null))
                .Returns(com.Object);
            com
                .Setup(x => x.ExecuteScalar())
                .Returns(1);

            var result = _database.ExecuteScalar(command);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ExecuteScalar_When_DbCommandThrowsException_ShouldThrow_DbCommandException()
        {
            var command = new DatabaseCommand();
            var com = _mockRepository.CreateIDbCommand();

            _connection
                .Setup(x => x.CreateCommand())
                .Returns(com.Object);
            com
                .Setup(x => x.ExecuteScalar())
                .Throws(new Exception("Error"));

            var ex = Assert.Throws<DatabaseCommandException>(() => _database.ExecuteScalar(command));

            Assert.AreEqual("Failed to execute scalar", ex.Message);
            Assert.AreEqual(command, ex.Command);
        }

        [Test]
        public void When_CannotConnectToDatabase_ShouldThrow_TimeoutException()
        {
            _connection
                .Setup(x => x.Open())
                .Throws(new Exception("Error"));

            var database = new TestDatabase(_connection.Object, _dataPopulator.Object, _commandBuilder.Object);

            var ex = Assert.Throws<Exception>(() => database.ExecuteNonQuery(new DatabaseCommand()));

            Assert.AreEqual("Error", ex.Message);
        }
    }
}