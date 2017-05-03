using System.Data;
using DatabaseConnections.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DatabaseTests
    {
        [SetUp]
        public void SetUp()
        {
            _connection = new Mock<IDbConnectionWrapper>();
            _database = new DatabaseStub(_connection.Object);
        }

        private Mock<IDbConnectionWrapper> _connection;
        private DatabaseStub _database;
        
        [Test]
        public void ExecuteNonQuery_ShouldCall_ExecuteNonQuery()
        {
            var command = new DatabaseCommand();

            _database.ExecuteNonQuery(command);

            _connection.Verify(x => x.ExecuteNonQuery(command), Times.Once);
        }

        [Test]
        public void ExecuteNonQuery_ShouldReturn_RowsAffected()
        {
            var command = new DatabaseCommand();

            _connection.Setup(x => x.ExecuteNonQuery(command)).Returns(1);

            var result = _database.ExecuteNonQuery(command);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ExecuteNonQuery_ShouldRaise_ExecutingEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executing += (x, y) => invoked = true;

            _database.ExecuteNonQuery(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteNonQuery_ShouldRaise_ExecutedEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executed += (x, y) => invoked = true;

            _database.ExecuteNonQuery(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteQuery_ShouldCall_ExecuteQuery()
        {
            var command = new DatabaseCommand();

            _database.ExecuteQuery(command);

            _connection.Verify(x => x.ExecuteQuery(command), Times.Once);
        }

        [Test]
        public void ExecuteQuery_ShouldReturn_DataSet()
        {
            var command = new DatabaseCommand();
            var ds = new DataSet();

            _connection.Setup(x => x.ExecuteQuery(command)).Returns(ds);

            var result = _database.ExecuteQuery(command);

            Assert.AreEqual(ds, result);
        }

        [Test]
        public void ExecuteQuery_ShouldRaise_ExecutingEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executing += (x, y) => invoked = true;

            _database.ExecuteQuery(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteQuery_ShouldRaise_ExecutedEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executed += (x, y) => invoked = true;

            _database.ExecuteQuery(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecutePagedQuery_ShouldCall_ExecuteQuery()
        {
            var command = new DatabaseCommand();

            _database.ExecutePagedQuery(command, 1, 1, "test");

            _connection.Verify(x => x.ExecuteQuery(command, 1, 1, "test"), Times.Once);
        }

        [Test]
        public void ExecutePagedQuery_ShouldReturn_DataSet()
        {
            var command = new DatabaseCommand();
            var ds = new DataSet();

            _connection.Setup(x => x.ExecuteQuery(command, 1, 1, "test")).Returns(ds);

            var result = _database.ExecutePagedQuery(command, 1, 1, "test");

            Assert.AreEqual(ds, result);
        }

        [Test]
        public void ExecutePagedQuery_ShouldRaise_ExecutingEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executing += (x, y) => invoked = true;

            _database.ExecutePagedQuery(command, 1, 1, "test");

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecutePagedQuery_ShouldRaise_ExecutedEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executed += (x, y) => invoked = true;

            _database.ExecutePagedQuery(command, 1, 1, "test");

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteScalar_ShouldCall_ExecuteScalar()
        {
            var command = new DatabaseCommand();

            _database.ExecuteScalar(command);

            _connection.Verify(x => x.ExecuteScalar(command), Times.Once);
        }

        [Test]
        public void ExecuteScalar_ShouldReturn_Scalar()
        {
            var command = new DatabaseCommand();

            _connection.Setup(x => x.ExecuteScalar(command)).Returns(1);

            var result = _database.ExecuteScalar(command);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ExecuteScalar_ShouldRaise_ExecutingEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executing += (x, y) => invoked = true;

            _database.ExecuteScalar(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteScalar_ShouldRaise_ExecutedEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executed += (x, y) => invoked = true;

            _database.ExecuteScalar(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteReader_ShouldCall_ExecuteReader()
        {
            var command = new DatabaseCommand();

            _database.ExecuteReader(command);

            _connection.Verify(x => x.ExecuteReader(command), Times.Once);
        }

        [Test]
        public void ExecuteReader_ShouldReturn_DataReader()
        {
            var command = new DatabaseCommand();
            var reader = new DataTableReader(new DataTable());

            _connection.Setup(x => x.ExecuteReader(command)).Returns(reader);

            var result = _database.ExecuteReader(command);

            Assert.AreEqual(reader, result);
        }

        [Test]
        public void ExecuteReader_ShouldRaise_ExecutingEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executing += (x, y) => invoked = true;

            _database.ExecuteReader(command);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void ExecuteReader_ShouldRaise_ExecutedEvent()
        {
            var command = new DatabaseCommand();
            var invoked = false;

            _database.Executed += (x, y) => invoked = true;

            _database.ExecuteReader(command);

            Assert.IsTrue(invoked);
        }
    }
}
