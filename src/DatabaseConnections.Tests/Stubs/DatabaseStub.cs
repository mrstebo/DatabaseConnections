namespace DatabaseConnections.Tests.Stubs
{
    internal class DatabaseStub : Database
    {
        public DatabaseStub(IDbConnectionWrapper connection) 
            : base(connection)
        {
        }
    }
}
