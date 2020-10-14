namespace Database.Communication
{
    public interface IDatabaseClientFactory<TDatabaseClient>
    {
        void CreateInitialClient(string address);
        TDatabaseClient GetClient();
    }
}