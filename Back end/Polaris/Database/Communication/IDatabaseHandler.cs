using System.Collections.Generic;

namespace Database.Communication
{
    public interface IDatabaseHandler<TModel>
    {
        void BulkInsert(IEnumerable<TModel> models, string sourceName);
        void Insert(TModel model, string sourceName);
        IEnumerable<TModel> FetchAll(string sourceName);
    }
}