using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace DatabaseConnections
{
    public abstract class Database
    {
        private readonly IDbConnectionWrapper _connection;

        protected Database(IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public int ExecuteNonQuery(DatabaseCommand command)
        {
            OnExecuting(new DatabaseCommandExecutingEventArgs(command));

            var result = Execute(() => _connection.ExecuteNonQuery(command));

            OnExecuted(new DatabaseCommandExecutedEventArgs(command, result.TimeTaken));

            return result.Value;
        }

        public DataSet ExecuteQuery(DatabaseCommand command)
        {
            OnExecuting(new DatabaseCommandExecutingEventArgs(command));

            var result = Execute(() => _connection.ExecuteQuery(command));

            OnExecuted(new DatabaseCommandExecutedEventArgs(command, result.TimeTaken));

            return result.Value;
        }

        public DataSet ExecutePagedQuery(DatabaseCommand command, int startRecord, int maxRecords, string tableName)
        {
            OnExecuting(new DatabaseCommandExecutingEventArgs(command));

            var result = Execute(() => _connection.ExecuteQuery(command, startRecord, maxRecords, tableName));

            OnExecuted(new DatabaseCommandExecutedEventArgs(command, result.TimeTaken));

            return result.Value;
        }

        public object ExecuteScalar(DatabaseCommand command)
        {
            OnExecuting(new DatabaseCommandExecutingEventArgs(command));

            var result = Execute(() => _connection.ExecuteScalar(command));

            OnExecuted(new DatabaseCommandExecutedEventArgs(command, result.TimeTaken));

            return result.Value;
        }

        public IDataReader ExecuteReader(DatabaseCommand command)
        {
            OnExecuting(new DatabaseCommandExecutingEventArgs(command));

            var result = Execute(() => _connection.ExecuteReader(command));

            OnExecuted(new DatabaseCommandExecutedEventArgs(command, result.TimeTaken));

            return result.Value;
        }

        public event EventHandler<DatabaseCommandExecutingEventArgs> Executing;
        public event EventHandler<DatabaseCommandExecutedEventArgs> Executed;

        protected virtual void OnExecuting(DatabaseCommandExecutingEventArgs e)
        {
            Executing?.Invoke(this, e);
        }

        protected virtual void OnExecuted(DatabaseCommandExecutedEventArgs e)
        {
            Executed?.Invoke(this, e);
        }

        private static TimedResult<T> Execute<T>(Func<T> func)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            return new TimedResult<T> {Value = func(), TimeTaken = stopwatch.Elapsed};
        }

        private struct TimedResult<T>
        {
            public T Value;
            public TimeSpan TimeTaken;
        }
    }
}