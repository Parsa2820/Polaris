using System.Collections.Generic;

namespace Database.Communication
{
    public interface IElasticHandler<TModel>
    {
        void BulkInsert(IEnumerable<TModel> models, string indexName);
        void Insert(TModel model, string indexName);
        IEnumerable<TModel> FetchAll(string indexName);
    }
}