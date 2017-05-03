using Npgsql;

namespace DatabaseConnections.Postgres
{
    public class PostgresDatabase : Database
    {
        public PostgresDatabase(NpgsqlConnection connection)
            : this(new NpgsqlConnectionWrapper(connection))
        {
        }

        internal PostgresDatabase(IDbConnectionWrapper connection) 
            : base(connection)
        {
        }
    }
}
