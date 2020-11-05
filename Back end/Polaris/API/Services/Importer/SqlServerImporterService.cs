using API.Services.Utils;
using Database.Communication;
using Models;

namespace API.Services.Importer
{
    public class SqlServerImporterService<TModel> : IImporterService<TModel>
        where TModel : class, IModel
    {
        private readonly IDatabaseHandler<TModel> _handler;

        public SqlServerImporterService(IDatabaseHandler<TModel> handler)
        {
            _handler = handler;
        }

        public void Import(string source, IStringParser<TModel> stringParser, string sourceName)
        {
            var list = stringParser.Parse(source);
            _handler.BulkInsert(list, sourceName);
        }
    }
}
