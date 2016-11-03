using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DatabaseConnections
{
    public static class DbParamConverter
    {
        public static IEnumerable<IDataParameter> ToDataParameters(IEnumerable<DbParam> parameters, IDbCommand com)
        {
            var results = new List<IDataParameter>();

            foreach (var parameter in parameters)
            {
                var p = com.CreateParameter();

                p.ParameterName = parameter.ParameterName;
                p.DbType = parameter.DbType;
                p.Value = parameter.Value;

                results.Add(p);
            }

            return results;
        }

        public static IEnumerable<DbParam> ToDbParams(IDataParameterCollection parameters)
        {
            return parameters
                .Cast<IDataParameter>()
                .Select(x => new DbParam(x.ParameterName, x.DbType, x.Value));
        }
    }
}