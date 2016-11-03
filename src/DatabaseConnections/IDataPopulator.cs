using System.Data;

namespace DatabaseConnections
{
    public interface IDataPopulator
    {
        DataSet Populate(IDbCommand command);
        DataSet Populate(IDbCommand command, int startRecord, int maxRecords, string tableName);
    }
}