using System.Data;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DbParamTests
    {
        [Test]
        public void Constructor_ShouldSet_Properties()
        {
            var parameter = new DbParam("@ParameterName", DbType.AnsiString, "My Parameter");

            Assert.AreEqual("@ParameterName", parameter.ParameterName);
            Assert.AreEqual(DbType.AnsiString, parameter.DbType);
            Assert.AreEqual("My Parameter", parameter.Value);
        }
    }
}