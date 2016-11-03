using System;
using System.Data;

namespace DatabaseConnections
{
    public static class DataRowExtensions
    {
        public static T GetValue<T>(this DataRow dataRow, string columnName, T defaultValue = default(T))
        {
            return ParseValue(dataRow[columnName], defaultValue);
        }

        public static T GetValue<T>(this DataRow dataRow, int columnIndex, T defaultValue = default(T))
        {
            return ParseValue(dataRow[columnIndex], defaultValue);
        }

        private static T ParseValue<T>(object value, T defaultValue)
        {
            try
            {
                return Convert.IsDBNull(value)
                    ? defaultValue
                    : (T) Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                // ignored
            }
            return defaultValue;
        }
    }
}