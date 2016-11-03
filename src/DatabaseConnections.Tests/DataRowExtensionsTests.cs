using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DataRowExtensionsTests
    {
        private static void AssertDataRowValue<T>(DataRow dr, string columnName, T expected)
        {
            Assert.AreEqual(expected, dr.GetValue<T>(columnName));
        }

        private static DataTable GetTestDataTable(IDictionary<DataColumn, IList<object>> data)
        {
            var dt = new DataTable();

            dt.Columns.AddRange(data.Keys.ToArray());

            for (var i = 0; i < data.First().Value.Count; i++)
            {
                var dr = dt.NewRow();

                data.Keys.ToList().ForEach(dc => dr[dc] = data[dc][i]);

                dt.Rows.Add(dr);
            }

            return dt;
        }

        [Test]
        public void GetValue_Given_ColumnIndex_ShouldReturn_Value()
        {
            var dt = GetTestDataTable(new Dictionary<DataColumn, IList<object>>
            {
                {new DataColumn("Test1", typeof(string)), new object[] {"Value 1"}},
                {new DataColumn("Test2", typeof(string)), new object[] {"Value 2"}}
            });
            var dr = dt.Rows[0];

            Assert.AreEqual("Value 1", dr.GetValue<string>(0));
            Assert.AreEqual("Value 2", dr.GetValue<string>(1));
        }

        [Test]
        public void GetValue_ShouldReturn_AppropriateType()
        {
            var testData = new Dictionary<Type, IList<object>>
            {
                {typeof(byte), new object[] {byte.MinValue, byte.MaxValue}},
                {typeof(short), new object[] {short.MinValue, short.MaxValue}},
                {typeof(int), new object[] {int.MinValue, int.MaxValue}},
                {typeof(long), new object[] {long.MinValue, long.MaxValue}},
                {typeof(sbyte), new object[] {sbyte.MinValue, sbyte.MaxValue}},
                {typeof(ushort), new object[] {ushort.MinValue, ushort.MaxValue}},
                {typeof(uint), new object[] {uint.MinValue, uint.MaxValue}},
                {typeof(ulong), new object[] {ulong.MinValue, ulong.MaxValue}},
                {typeof(string), new object[] {null, "TEST STRING"}},
                {typeof(DateTime), new object[] {DateTime.MinValue, DateTime.MaxValue}}
            };
            var dt = GetTestDataTable(testData.ToDictionary(x => new DataColumn(x.Key.Name, x.Key), y => y.Value));

            for (var i = 0; i < dt.Rows.Count; i++)
                testData.Keys
                    .ToList()
                    .ForEach(t => AssertDataRowValue(dt.Rows[i], t.Name, testData[t][i]));
        }

        [Test]
        public void GetValue_With_InvalidType_ShouldReturn_DefaultValue()
        {
            var dt = GetTestDataTable(new Dictionary<DataColumn, IList<object>>
            {
                {new DataColumn("Test", typeof(string)), new object[] {"I am not a number! I am a free man!", null}}
            });

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                AssertDataRowValue(dt.Rows[i], "Test", default(byte));
                AssertDataRowValue(dt.Rows[i], "Test", default(byte));
                AssertDataRowValue(dt.Rows[i], "Test", default(short));
                AssertDataRowValue(dt.Rows[i], "Test", default(int));
                AssertDataRowValue(dt.Rows[i], "Test", default(long));
                AssertDataRowValue(dt.Rows[i], "Test", default(sbyte));
                AssertDataRowValue(dt.Rows[i], "Test", default(ushort));
                AssertDataRowValue(dt.Rows[i], "Test", default(uint));
                AssertDataRowValue(dt.Rows[i], "Test", default(ulong));
                AssertDataRowValue(dt.Rows[i], "Test", default(DateTime));
            }
        }

        [Test]
        public void GetValue_With_TypeOfDateTime_On_NullValue_ShouldReturn_DefaultDateTime()
        {
            var dt = GetTestDataTable(new Dictionary<DataColumn, IList<object>>
            {
                {new DataColumn("Test", typeof(string)), new object[] {null}}
            });

            for (var i = 0; i < dt.Rows.Count; i++)
                AssertDataRowValue(dt.Rows[i], "Test", default(DateTime));
        }

        [Test]
        public void GetValue_With_TypeOfString_ShouldReturn_StringRepresentationOfValue()
        {
            var testData = new Dictionary<Type, IList<object>>
            {
                {typeof(byte), new object[] {byte.MinValue, byte.MaxValue}},
                {typeof(short), new object[] {short.MinValue, short.MaxValue}},
                {typeof(int), new object[] {int.MinValue, int.MaxValue}},
                {typeof(long), new object[] {long.MinValue, long.MaxValue}},
                {typeof(sbyte), new object[] {sbyte.MinValue, sbyte.MaxValue}},
                {typeof(ushort), new object[] {ushort.MinValue, ushort.MaxValue}},
                {typeof(uint), new object[] {uint.MinValue, uint.MaxValue}},
                {typeof(ulong), new object[] {ulong.MinValue, ulong.MaxValue}}
            };
            var dt = GetTestDataTable(testData.ToDictionary(x => new DataColumn(x.Key.Name, x.Key), y => y.Value));

            for (var i = 0; i < dt.Rows.Count; i++)
                foreach (var t in testData.Keys)
                    AssertDataRowValue(dt.Rows[i], t.Name, testData[t][i].ToString());
        }
    }
}