using Models.Response;
using System.Collections.Generic;

namespace Database.Communication
{
    public interface IDatabaseHandler<TModel>
    {
        void BulkInsert(IEnumerable<TModel> models, string sourceName);
        void Insert(TModel model, string sourceName);
        IEnumerable<TModel> FetchAll(string sourceName);
        void CheckSource(string sourceName, bool recreate = false);
        IEnumerable<TModel> RetrieveQueryDocumentsByFilter(
            string[] filter,
            string indexName,
            Pagination pagination = null
        );
    }
}