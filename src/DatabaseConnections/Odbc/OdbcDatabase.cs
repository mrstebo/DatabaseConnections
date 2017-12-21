using System.Data.Odbc;

namespace DatabaseConnections.Odbc
{
    public class OdbcDatabase : Database
    {
        public OdbcDatabase(OdbcConnection connection)
            : this(new OdbcConnectionWrapper(connection))
        {
        }

        internal OdbcDatabase(IDbConnectionWrapper connection)
            : base(connection)
        {
        }
    }
}
