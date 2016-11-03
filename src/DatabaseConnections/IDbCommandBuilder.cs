using System.Data;

namespace DatabaseConnections
{
    public interface IDbCommandBuilder
    {
        IDbCommand BuildCommand(DatabaseCommand command, IDbConnection con, IDbTransaction transaction = null);
    }
}