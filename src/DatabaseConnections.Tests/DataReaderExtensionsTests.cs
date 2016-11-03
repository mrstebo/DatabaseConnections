using System.Data;
using Moq;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    public class DataReaderExtensionsTests
    {
        private Mock<IDatabase> _database;

        private DataTable _testData;

        [SetUp]
        public void SetUp()
        {
            _database = new Mock<IDatabase>();
            _testData = new DataTable("test");
            _testData.Columns.Add("ID", typeof(int));
            _testData.Columns.Add("Name", typeof(string));
        }

        private void AddRowToTestData(int id, string name)
        {
            var dr = _testData.NewRow();
            dr["ID"] = id;
            dr["Name"] = name;
            _testData.Rows.Add(dr);
        }

        [Test]
        public void GetValue_ShouldReturn_ValueForColumn()
        {
            var command = new DatabaseCommand();

            _database
                .Setup(x => x.ExecuteReader(command))
                .Returns(new DataTableReader(_testData));

            AddRowToTestData(1, "Test User");

            using (var reader = _database.Object.ExecuteReader(command))
            {
                Assert.IsTrue(reader.Read());

                var result = reader.GetValue<string>("Name");

                Assert.AreEqual("Test User", result);
            }
        }

        [Test]
        public void GetValue_When_ColumnDoesNotExist_ShouldReturn_DefaultValue()
        {
            var command = new DatabaseCommand();

            _database
                .Setup(x => x.ExecuteReader(command))
                .Returns(new DataTableReader(_testData));

            AddRowToTestData(1, "Test User");

            using (var reader = _database.Object.ExecuteReader(command))
            {
                Assert.IsTrue(reader.Read());

                var result = reader.GetValue("Description", "DEFAULT");

                Assert.AreEqual("DEFAULT", result);
            }
        }
    }
}