using Database.Exceptions;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.Communication.MicrosoftSqlServer
{
    class MSqlEntityHandler<TModel, TType> : MSqlHandler<TModel>, IEntityHandler<TModel, TType>
    where TModel : Entity<TType>, new()
    {
        public void DeleteEntity(TType id, string sourceName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> GetEntities(TType[] ids, string sourceName)
        {
            StringBuilder idsCondition = new StringBuilder();

            foreach (var id in ids)
                idsCondition.Append($"Id = {id} OR ");

            idsCondition.Length -= 4; // To remove last redundant OR and sapces around it
            var queryString = $"SELECT * FROM {sourceName} WHERE {idsCondition}";
            var connectionString = MSqlClientFactory.GetInstance().GetClient();
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return null;
            }
        }

        public TModel GetEntity(TType id, string sourceName)
        {
            var queryString = $"SELECT * FROM {sourceName} WHERE Id = {id}";
            var connectionString = MSqlClientFactory.GetInstance().GetClient();
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                //var response = connection.Query<TModel>(queryString);
                var response = new List<TModel>();
                if (response.Any())
                {
                    throw new EntityNotFoundException($"Entity with id: \"{id}\" not found in table \"{sourceName}\"");
                }
                return response.First();
            }
        }

        public void UpdateEntity(TModel newEntity, string sourceName)
        {
            throw new NotImplementedException();
        }
    }
}
