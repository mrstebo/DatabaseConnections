using System.Data;

namespace DatabaseConnections
{
    public interface IDbCommandBuilder
    {
        IDbCommand BuildCommand(DatabaseCommand command, IDbConnection con, IDbTransaction transaction = null);
    }

    public class DbCommandBuilder : IDbCommandBuilder
    {
        public IDbCommand BuildCommand(DatabaseCommand command, IDbConnection con,
            IDbTransaction transaction = null)
        {
            var com = con.CreateCommand();

            com.Transaction = transaction;
            com.CommandText = command.CommandText;

            foreach (var parameter in command.Parameters ?? new DbParam[] {})
            {
                com.Parameters.Add(ConvertToDataParameter(parameter, com));
            }

            return com;
        }

        protected virtual IDataParameter ConvertToDataParameter(DbParam parameter, IDbCommand com)
        {
            var p = com.CreateParameter();

            p.ParameterName = parameter.ParameterName;
            p.DbType = parameter.DbType;
            p.Value = parameter.Value;

            return p;

        }
    }
}