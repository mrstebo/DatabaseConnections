using System.Data;

namespace DatabaseConnections
{
    public static class DatabaseExtensions
    {
        public static int ExecuteNonQuery(this Database database, string commandText, params DbParam[] parameters)
        {
            return database.ExecuteNonQuery(new DatabaseCommand
            {
                CommandText = commandText,
                Parameters = parameters
            });
        }

        public static DataSet ExecuteQuery(this Database database, string commandText, params DbParam[] parameters)
        {
            return database.ExecuteQuery(new DatabaseCommand
            {
                CommandText = commandText,
                Parameters = parameters
            });
        }

        public static DataSet ExecutePagedQuery(this Database database, int startRecord, int maxRecords, string tableName,
            string commandText, params DbParam[] parameters)
        {
            return database.ExecutePagedQuery(new DatabaseCommand
            {
                CommandText = commandText,
                Parameters = parameters
            }, startRecord, maxRecords, tableName);
        }

        public static object ExecuteScalar(this Database database, string commandText, params DbParam[] parameters)
        {
            return database.ExecuteScalar(new DatabaseCommand
            {
                CommandText = commandText,
                Parameters = parameters
            });
        }

        public static IDataReader ExecuteReader(this Database database, string commandText,
            params DbParam[] parameters)
        {
            return database.ExecuteReader(new DatabaseCommand
            {
                CommandText = commandText,
                Parameters = parameters
            });
        }
    }
}