namespace DatabaseConnections
{
    public interface IDatabaseCommandInteceptor
    {
        void Intercept(IDatabase database, DatabaseCommand command);
    }
}