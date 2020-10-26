namespace Database.Communication.MicrosoftSqlServer
{
    public class MSqlClientFactory : IDatabaseClientFactory<string>
    {
        public static MSqlClientFactory singletonInstance {get;} = new MSqlClientFactory();
        private string client = null;

        private MSqlClientFactory()
        {
        }

        public void CreateInitialClient(string address)
        {
            client = address;
        }

        public string GetClient()
        {
            return client;
        }
    }
}
