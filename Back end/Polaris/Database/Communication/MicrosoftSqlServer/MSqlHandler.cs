using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.Communication.MicrosoftSqlServer
{
    public class MSqlHandler<TModel> : IDatabaseHandler<TModel>
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
            var connectionString = MSqlClientFactory.GetInstance().GetClient();
            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();
            names.Append("(");
            values.Append("(");
            var properties = model.GetType().GetProperties();

            names.Append(properties[0].Name);
            values.Append(properties[0].GetValue(model));

            for(int i = 1; i < properties.Length; i++)
            {
                names.Append(",");
                names.Append(properties[i].Name);
                values.Append(",");
                values.Append(properties[i].GetValue(model));
            }

            names.Append(")");
            values.Append(")");

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute($"INSERT INTO {sourceName}({names}) VALUES {values}", model);
            }
        }
    }
}
