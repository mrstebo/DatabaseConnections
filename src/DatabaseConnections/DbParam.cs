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

        public string ParameterName { get; private set; }
        public DbType DbType { get; private set; }
        public object Value { get; private set; }
    }
}