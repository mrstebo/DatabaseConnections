using System;
using System.Data;
using System.Linq;

namespace DatabaseConnections
{
    public static class DataReaderExtensions
    {
        public static T GetValue<T>(this IDataReader reader, string columnName, T defaultValue = default(T))
        {
            return ContainsColumn(reader, columnName)
                ? (T) Convert.ChangeType(reader[columnName], typeof(T))
                : defaultValue;
        }

        private static bool ContainsColumn(IDataRecord reader, string columnName)
        {
            return Enumerable.Range(0, reader.FieldCount)
                .Any(i => reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }
    }
}