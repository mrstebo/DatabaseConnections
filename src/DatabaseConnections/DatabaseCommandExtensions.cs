using System;

namespace DatabaseConnections
{
    public static class DatabaseCommandExtensions
    {
        public static string Compile(this DatabaseCommand command)
        {
            var commandText = command.CommandText;

            foreach (var parameter in command.Parameters)
            {
                var value = Convert.ToString(parameter.Value);
                var replacement = parameter.Value is string
                    ? CreateStringReplacement(value)
                    : value;

                commandText = commandText.Replace(parameter.ParameterName, replacement);
            }

            return commandText;
        }

        private static string CreateStringReplacement(string value)
        {
            return string.Format("'{0}'", value.Replace("'", "\\'"));
        }
    }
}