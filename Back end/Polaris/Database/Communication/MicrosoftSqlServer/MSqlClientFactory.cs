using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Database.Communication.MicrosoftSqlServer
{
    public class MSqlClientFactory : IDatabaseClientFactory<string>
    {
        private static MSqlClientFactory singletonInstance = new MSqlClientFactory();
        private string client = null;

        private MSqlClientFactory()
        {
        }

        public static MSqlClientFactory GetInstance()
        {
            return singletonInstance;
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
