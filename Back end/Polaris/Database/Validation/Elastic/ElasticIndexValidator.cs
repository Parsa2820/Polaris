using Database.Communication.Elastic.Nest;
using Database.Exceptions.Elastic;
using Nest;

namespace Database.Validation.Elastic
{
    public class ElasticIndexValidator
    {
        private static IElasticClient elasticClient = NestClientFactory.GetInstance().GetClient();

        public static void ValidateIndex(string indexName)
        {
            if (elasticClient.Indices.Exists(indexName).Exists)
                return;
            throw new InvalidElasticIndexException($"\"{indexName}\" index does not exist");
        }
    }
}