using System.Data.SQLite;

namespace DatabaseConnections.SQLite
{
    public class SQLiteDatabase : Database
    {
        public SQLiteDatabase(SQLiteConnection connection)
            : this(new SQLiteConnectionWrapper(connection))
        {
        }

        internal SQLiteDatabase(IDbConnectionWrapper connection) 
            : base(connection)
        {
        }
    }
}
