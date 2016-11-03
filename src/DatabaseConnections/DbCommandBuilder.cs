using System.Data;
using System.Linq;

namespace DatabaseConnections
{
    public class DbCommandBuilder : IDbCommandBuilder
    {
        public IDbCommand BuildCommand(DatabaseCommand command, IDbConnection con,
            IDbTransaction transaction = null)
        {
            var com = con.CreateCommand();
            var parameters = command.Parameters ?? new DbParam[] {};

            com.Transaction = transaction;
            com.CommandText = command.CommandText;

            parameters
                .Select(x => CreateDataParameter(x, com))
                .ToList()
                .ForEach(x => com.Parameters.Add(x));
            
            return com;
        }

        private static IDataParameter CreateDataParameter(DbParam parameter, IDbCommand com)
        {
            var p = com.CreateParameter();

            p.ParameterName = parameter.ParameterName;
            p.DbType = parameter.DbType;
            p.Value = parameter.Value;

            return p;

        }
    }
}