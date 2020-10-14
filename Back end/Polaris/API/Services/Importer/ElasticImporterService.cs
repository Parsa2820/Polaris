using API.Services.Utils;
using Database.Communication;
using Models;

namespace API.Services.Importer
{
    public class ElasticImporterService<TModel> : IImporterService<TModel> where TModel : class, IModel
    {
        private readonly IDatabaseHandler<TModel> _handler;

        public ElasticImporterService(IDatabaseHandler<TModel> handler)
        {
            _handler = handler;
        }

        public void Import(string source, IStringParser<TModel> stringParser, string indexName)
        {
            var list = stringParser.Parse(source);
            _handler.BulkInsert(list, indexName);
        }
    }
}