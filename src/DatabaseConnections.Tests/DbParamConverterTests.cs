using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DbParamConverterTests
    {
        [Test]
        [TestCase(typeof(OleDbConnection))]
        [TestCase(typeof(OdbcConnection))]
        public void ToDataParameters_ShouldReturn_DataParameters(Type connectionType)
        {
            using (var con = (IDbConnection) Activator.CreateInstance(connectionType))
            {
                using (var com = con.CreateCommand())
                {
                    var parameter = new DbParam("@Test", DbType.String, "Test Value");

                    var results = DbParamConverter.ToDataParameters(new[] {parameter}, com).ToArray();

                    Assert.AreEqual(1, results.Length);
                    Assert.AreEqual(parameter.ParameterName, results[0].ParameterName);
                    Assert.AreEqual(parameter.DbType, results[0].DbType);
                    Assert.AreEqual(parameter.Value, results[0].Value);
                }
            }
        }

        [Test]
        [TestCase(typeof(OleDbConnection))]
        [TestCase(typeof(OdbcConnection))]
        public void ToDbParams_ShouldReturn_DbParams(Type connectionType)
        {
            using (var con = (IDbConnection) Activator.CreateInstance(connectionType))
            {
                using (var com = con.CreateCommand())
                {
                    var parameter = com.CreateParameter();

                    parameter.ParameterName = "@Test";
                    parameter.DbType = DbType.String;
                    parameter.Value = "Test Value";

                    com.Parameters.Add(parameter);

                    var results = DbParamConverter.ToDbParams(com.Parameters).ToArray();

                    Assert.AreEqual(1, results.Length);
                    Assert.AreEqual(parameter.ParameterName, results[0].ParameterName);
                    Assert.AreEqual(parameter.DbType, results[0].DbType);
                    Assert.AreEqual(parameter.Value, results[0].Value);
                }
            }
        }
    }
}