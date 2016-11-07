namespace DatabaseConnections
{
    public interface IDatabaseCommandInterceptor
    {
        void Intercept(DatabaseCommand command, IDatabase database);
    }
}