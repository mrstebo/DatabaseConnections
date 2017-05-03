using System.Data.OleDb;

namespace DatabaseConnections.OleDb
{
    public class OleDbDatabase : Database
    {
        public OleDbDatabase(OleDbConnection connection) 
            : this(new OleDbConnectionWrapper(connection))
        {
        }

        internal OleDbDatabase(IDbConnectionWrapper connection)
            : base(connection)
        {
        }
    }
}
