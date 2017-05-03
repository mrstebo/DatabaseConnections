using System.Data;

namespace DatabaseConnections
{
    public interface IDbConnectionWrapper
    {
        int ExecuteNonQuery(DatabaseCommand command);
        DataSet ExecuteQuery(DatabaseCommand command);
        DataSet ExecuteQuery(DatabaseCommand command, int startRecord, int maxRecords, string tableName);
        object ExecuteScalar(DatabaseCommand command);
        IDataReader ExecuteReader(DatabaseCommand command);
    }
}