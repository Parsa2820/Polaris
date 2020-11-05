

namespace Database.Communication.MicrosoftSqlServer
{
    public class MSqlClientFactory : IDatabaseClientFactory<string>
    {
        public static MSqlClientFactory Instance { get; } = new MSqlClientFactory();
        private string client = null; // Sql Server client is a connection string

        private MSqlClientFactory()
        {
        }

        public void CreateInitialClient(string dbDescription)
        {
            client = dbDescription;
        }

        public string GetClient()
        {
            return client;
        }
    }
}
