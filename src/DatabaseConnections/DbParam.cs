using System.Data;

namespace DatabaseConnections
{
    public class DbParam
    {
        public DbParam(string parameterName, object value)
            : this(parameterName, DbType.Object, value)
        {
        }

        public DbParam(string parameterName, DbType dbType, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
        }

        public string ParameterName { get; }
        public DbType DbType { get; }
        public object Value { get; }
    }
}