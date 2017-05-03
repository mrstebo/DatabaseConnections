using System.Data;
using System.Data.OleDb;

namespace DatabaseConnections.OleDb
{
    internal class OleDbConnectionWrapper : IDbConnectionWrapper
    {
        private readonly OleDbConnection _connection;

        public OleDbConnectionWrapper(OleDbConnection connection)
        {
            _connection = connection;
        }

        public int ExecuteNonQuery(DatabaseCommand command)
        {
            using (var com = CreateCommand(command))
            {
                return com.ExecuteNonQuery();
            }
        }

        public DataSet ExecuteQuery(DatabaseCommand command)
        {
            var ds = new DataSet();

            using (var com = CreateCommand(command))
            {
                using (var da = new OleDbDataAdapter(com))
                {
                    da.Fill(ds);
                }
            }

            return ds;
        }

        public DataSet ExecuteQuery(DatabaseCommand command, int startRecord, int maxRecords, string tableName)
        {
            var ds = new DataSet();

            using (var com = CreateCommand(command))
            {
                using (var da = new OleDbDataAdapter(com))
                {
                    da.Fill(ds, startRecord, maxRecords, tableName);
                }
            }

            return ds;
        }

        public object ExecuteScalar(DatabaseCommand command)
        {
            using (var com = CreateCommand(command))
            {
                return com.ExecuteScalar();
            }
        }

        public IDataReader ExecuteReader(DatabaseCommand command)
        {
            var com = CreateCommand(command);

            return com.ExecuteReader();
        }

        private OleDbCommand CreateCommand(DatabaseCommand command)
        {
            var com = _connection.CreateCommand();

            com.CommandText = command.CommandText;

            foreach (var parameter in command.Parameters)
                com.Parameters.Add(CreateParameter(parameter, com));

            return com;
        }

        private static OleDbParameter CreateParameter(DbParam parameter, OleDbCommand com)
        {
            var p = com.CreateParameter();

            p.ParameterName = parameter.ParameterName;
            p.DbType = parameter.DbType;
            p.Value = parameter.Value;

            return p;
        }
    }
}
