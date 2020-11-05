namespace Database.Communication
{
    public interface IDatabaseClientFactory<TDatabaseClient>
    {
        void CreateInitialClient(string dbDescription);
        TDatabaseClient GetClient();
    }
}