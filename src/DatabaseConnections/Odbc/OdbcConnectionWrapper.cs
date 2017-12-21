using System.Data;
using System.Data.Odbc;

namespace DatabaseConnections.Odbc
{
    internal class OdbcConnectionWrapper : IDbConnectionWrapper
    {
        private readonly OdbcConnection _connection;

        public OdbcConnectionWrapper(OdbcConnection connection)
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
                using (var da = new OdbcDataAdapter(com))
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
                using (var da = new OdbcDataAdapter(com))
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

        private OdbcCommand CreateCommand(DatabaseCommand command)
        {
            var com = _connection.CreateCommand();

            com.CommandText = command.CommandText;

            foreach (var parameter in command.Parameters)
                com.Parameters.Add(CreateParameter(parameter, com));

            return com;
        }

        private static OdbcParameter CreateParameter(DbParam parameter, OdbcCommand com)
        {
            var p = com.CreateParameter();

            p.ParameterName = parameter.ParameterName;
            p.DbType = parameter.DbType;
            p.Value = parameter.Value;

            return p;
        }
    }
}
