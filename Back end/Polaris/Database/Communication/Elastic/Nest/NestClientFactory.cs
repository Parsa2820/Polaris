using Database.Exceptions;
using Nest;
using System;

namespace Database.Communication.Elastic.Nest
{
    public class NestClientFactory : IElasticClientFactory<IElasticClient>
    {
        private static NestClientFactory singletonInstance = new NestClientFactory();
        private IElasticClient client = null;

        private NestClientFactory()
        {
        }

        public void CreateInitialClient(string address)
        {
            var uri = new Uri(address);
            var connectionSettings = new ConnectionSettings(uri);
            client = new ElasticClient(connectionSettings);
        }

        public IElasticClient GetElasticClient()
        {
            if (client == null)
            {
                throw new ClientNotInitializedException();
            }
            return client;
        }

        public static NestClientFactory GetInstance()
        {
            return singletonInstance;
        }
    }
}