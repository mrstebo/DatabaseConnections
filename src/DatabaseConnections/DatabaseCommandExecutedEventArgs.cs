using System;

namespace DatabaseConnections
{
    public sealed class DatabaseCommandExecutedEventArgs : EventArgs
    {
        public DatabaseCommandExecutedEventArgs(DatabaseCommand command, TimeSpan timeTaken)
        {
            Command = command;
            TimeTaken = timeTaken;
        }

        public DatabaseCommand Command { get; private set; }
        public TimeSpan TimeTaken { get; private set; }
    }
}