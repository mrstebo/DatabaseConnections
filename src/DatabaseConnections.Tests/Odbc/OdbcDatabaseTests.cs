using System.Data.Odbc;
using DatabaseConnections.Odbc;
using NUnit.Framework;

namespace DatabaseConnections.Tests.Odbc
{
    [TestFixture]
    [Parallelizable]
    public class OdbcDatabaseTests
    {
        [Test]
        public void Constructor_ShouldNot_ThrowException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new OdbcDatabase(new OdbcConnection()));
        }
    }
}
