using Dapper;
using Database.Exceptions.MicrosoftSqlServer;
using Database.Validation.MicrosoftSqlServer;
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
            DataTable dataTable = new DataTable();
            var properties = models.First().GetType().GetProperties();

            foreach (var property in properties)
                dataTable.Columns.Add(new DataColumn(property.Name, property.PropertyType));

            foreach (var model in models)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (var property in properties)
                    dataRow[property.Name] = property.GetValue(model);

                dataTable.Rows.Add(dataRow);
            }

            var connectionString = MSqlClientFactory.GetInstance().GetClient();

            var connection = new SqlConnection(connectionString);
            SqlBulkCopy sqlBulk = new SqlBulkCopy(connection);
            sqlBulk.DestinationTableName = sourceName;

            foreach (var property in properties)
                sqlBulk.ColumnMappings.Add(property.Name, property.Name);

            connection.Open();
            sqlBulk.WriteToServer(dataTable);
            connection.Close();
        }

        public IEnumerable<TModel> FetchAll(string sourceName)
        {
            var queryString = $"SELECT * FROM {sourceName}";
            var connectionString = MSqlClientFactory.GetInstance().GetClient();
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<TModel>(queryString).ToList();
            }
        }

        public void Insert(TModel model, string sourceName)
        {
            BulkInsert(new List<TModel> { model }, sourceName);
        }

        public void CheckSource(string sourceName, bool recreate)
        {
            try
            {
                MicrosoftSqlTableValidator.ValidateTable(sourceName);
            }
            catch (InvalidSqlTableException e)
            {
                if (recreate)
                {
                    // Todo : Create new table from TModel
                }
                else
                    throw e;
            }
        }
    }
}
