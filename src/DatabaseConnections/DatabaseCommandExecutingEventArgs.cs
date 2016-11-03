using System;

namespace DatabaseConnections
{
    public sealed class DatabaseCommandExecutingEventArgs : EventArgs
    {
        public DatabaseCommandExecutingEventArgs(DatabaseCommand command)
        {
            Command = command;
        }

        public DatabaseCommand Command { get; private set; }
    }
}