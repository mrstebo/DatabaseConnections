using System;

namespace DatabaseConnections
{
    public sealed class DatabaseCommandException : Exception
    {
        public DatabaseCommandException(DatabaseCommand command, string message, Exception ex)
            : base(message, ex)
        {
            Command = command;
        }

        public DatabaseCommand Command { get; private set; }
    }
}