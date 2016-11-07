using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace DatabaseConnections
{
    public interface IDatabase
    {
        string ConnectionString { get; }

        IList<IDatabaseCommandInterceptor> Inteceptors { get; }

        int ExecuteNonQuery(DatabaseCommand command);
        int ExecuteNonQueries(IEnumerable<DatabaseCommand> commands);
        DataSet ExecuteQuery(DatabaseCommand command);
        DataSet ExecuteQuery(DatabaseCommand command, int startRecord, int maxRecords, string tableName);
        object ExecuteScalar(DatabaseCommand command);
        IDataReader ExecuteReader(DatabaseCommand command);

        event EventHandler<DatabaseCommandExecutingEventArgs> Executing;
        event EventHandler<DatabaseCommandExecutedEventArgs> Executed;
    }

    public abstract class Database : IDatabase
    {
        private readonly IDbConnection _connection;
        private readonly IDataPopulator _dataPopulator;
        private readonly IDbCommandBuilder _commandBuilder;
        private readonly IList<IDatabaseCommandInterceptor> _inteceptors = new List<IDatabaseCommandInterceptor>();

        protected Database(
            IDbConnection connection,
            IDataPopulator dataPopulator,
            IDbCommandBuilder commandBuilder)
        {
            _connection = connection;
            _dataPopulator = dataPopulator;
            _commandBuilder = commandBuilder;
        }

        public string ConnectionString
        {
            get { return _connection.ConnectionString; }
        }

        public IList<IDatabaseCommandInterceptor> Inteceptors
        {
            get { return _inteceptors; }
        }

        public int ExecuteNonQuery(DatabaseCommand command)
        {
            var con = OpenConnection();

            return BeginExecute(command, () =>
            {
                using (var com = _commandBuilder.BuildCommand(command, con))
                {
                    try
                    {
                        return com.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new DatabaseCommandException(command, "Failed to execute non query", ex);
                    }
                }
            });
        }

        public int ExecuteNonQueries(IEnumerable<DatabaseCommand> commands)
        {
            var result = 0;
            var con = OpenConnection();

            using (var transaction = con.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    result += (commands ?? new DatabaseCommand[] {}).Sum(command => BeginExecute(command, () =>
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        using (var com = _commandBuilder.BuildCommand(command, con, transaction))
                        {
                            try
                            {
                                return com.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw new DatabaseCommandException(command, "Failed to execute non queries", ex);
                            }
                        }
                    }));
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public DataSet ExecuteQuery(DatabaseCommand command)
        {
            var con = OpenConnection();

            return BeginExecute(command, () =>
            {
                using (var com = _commandBuilder.BuildCommand(command, con))
                {
                    try
                    {
                        return _dataPopulator.Populate(com);
                    }
                    catch (Exception ex)
                    {
                        throw new DatabaseCommandException(command, "Failed to execute query", ex);
                    }
                }
            });
        }

        public DataSet ExecuteQuery(DatabaseCommand command, int startRecord, int maxRecords, string tableName)
        {
            var con = OpenConnection();

            return BeginExecute(command, () =>
            {
                using (var com = _commandBuilder.BuildCommand(command, con))
                {
                    try
                    {
                        return _dataPopulator.Populate(com, startRecord, maxRecords, tableName);
                    }
                    catch (Exception ex)
                    {
                        throw new DatabaseCommandException(command, "Failed to execute paged query", ex);
                    }
                }
            });
        }

        public object ExecuteScalar(DatabaseCommand command)
        {
            var con = OpenConnection();

            return BeginExecute(command, () =>
            {
                using (var com = _commandBuilder.BuildCommand(command, con))
                {
                    try
                    {
                        return com.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new DatabaseCommandException(command, "Failed to execute scalar", ex);
                    }
                }
            });
        }

        public IDataReader ExecuteReader(DatabaseCommand command)
        {
            var con = OpenConnection();

            return BeginExecute(command, () =>
            {
                using (var com = _commandBuilder.BuildCommand(command, con))
                {
                    try
                    {
                        return com.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        throw new DatabaseCommandException(command, "Failed to execute reader", ex);
                    }
                }
            });
        }

        public event EventHandler<DatabaseCommandExecutingEventArgs> Executing;
        public event EventHandler<DatabaseCommandExecutedEventArgs> Executed;

        private T BeginExecute<T>(DatabaseCommand command, Func<T> func)
        {
            var stopwatch = new Stopwatch();

            RunInterceptors(command);

            OnExecuting(command);
            stopwatch.Start();

            var result = func();

            stopwatch.Stop();
            OnExecuted(command, stopwatch.Elapsed);

            return result;
        }

        private void RunInterceptors(DatabaseCommand command)
        {
            foreach (var inteceptor in Inteceptors)
                inteceptor.Intercept(command, this);
        }

        protected virtual IDbConnection OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            return _connection;
        }

        protected virtual void OnExecuting(DatabaseCommand command)
        {
            if (Executing != null)
                Executing(this, new DatabaseCommandExecutingEventArgs(command));
        }

        protected virtual void OnExecuted(DatabaseCommand command, TimeSpan timeTaken)
        {
            if (Executed != null)
                Executed(this, new DatabaseCommandExecutedEventArgs(command, timeTaken));
        }
    }
}