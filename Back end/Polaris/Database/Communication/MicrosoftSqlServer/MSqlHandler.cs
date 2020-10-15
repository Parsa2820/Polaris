using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Database.Communication.MicrosoftSqlServer
{
    class MSqlHandler<TModel> : IDatabaseHandler<TModel>
    {
        public void BulkInsert(IEnumerable<TModel> models, string sourceName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> FetchAll(string sourceName)
        {
            var connectionString = MSqlClientFactory.GetInstance().GetClient();
            var queryString = $"SELECT * FROM {sourceName}";
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<TModel>(queryString).ToList();
            }
        }

        public void Insert(TModel model, string sourceName)
        {
            throw new NotImplementedException();
        }
    }
}
