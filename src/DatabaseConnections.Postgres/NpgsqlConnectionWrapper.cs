using System.Data;
using Npgsql;

namespace DatabaseConnections.Postgres
{
    internal class NpgsqlConnectionWrapper : IDbConnectionWrapper
    {
        private readonly NpgsqlConnection _connection;

        public NpgsqlConnectionWrapper(NpgsqlConnection connection)
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
                using (var da = new NpgsqlDataAdapter(com))
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
                using (var da = new NpgsqlDataAdapter(com))
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

        private NpgsqlCommand CreateCommand(DatabaseCommand command)
        {
            var com = _connection.CreateCommand();

            com.CommandText = command.CommandText;

            foreach (var parameter in command.Parameters)
                com.Parameters.Add(CreateParameter(parameter, com));

            return com;
        }

        private static NpgsqlParameter CreateParameter(DbParam parameter, NpgsqlCommand com)
        {
            var p = com.CreateParameter();

            p.ParameterName = parameter.ParameterName;
            p.DbType = parameter.DbType;
            p.Value = parameter.Value;

            return p;
        }
    }
}
