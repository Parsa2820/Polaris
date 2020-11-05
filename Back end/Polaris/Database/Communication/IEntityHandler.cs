using Models;
using System.Collections.Generic;

namespace Database.Communication
{
    public interface IEntityHandler<TModel, TType> : IDatabaseHandler<TModel>
    where TModel : Entity<TType>
    {
        TModel GetEntity(TType id, string sourceName);
        IEnumerable<TModel> GetEntities(TType[] ids, string sourceName);
        void UpdateEntity(TModel newEntity, string sourceName);
        void DeleteEntity(TType id, string sourceName);
    }
}