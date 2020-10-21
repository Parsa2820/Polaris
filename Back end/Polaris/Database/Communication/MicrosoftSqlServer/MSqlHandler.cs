using Database.Exceptions.MicrosoftSqlServer;
using Database.Validation.MicrosoftSqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Database.Communication.MicrosoftSqlServer
{
    public class MSqlHandler<TModel> : IDatabaseHandler<TModel>
        where TModel : new()
    {
        protected static Dictionary<Type, SqlDbType> typeToSqlDbType;
        protected string connectionString;

        public MSqlHandler()
        {
            connectionString = MSqlClientFactory.GetInstance().GetClient();
            InitTypeToSqlDbType();
        }

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

            var connection = new SqlConnection(connectionString);
            SqlBulkCopy sqlBulk = new SqlBulkCopy(connection);
            sqlBulk.DestinationTableName = sourceName;

            foreach (var property in properties)
                sqlBulk.ColumnMappings.Add(property.Name, property.Name);

            connection.Open();
            sqlBulk.WriteToServer(dataTable);
            dataTable.Dispose();
            sqlBulk.Close();
            connection.Close();
        }

        public IEnumerable<TModel> FetchAll(string sourceName)
        {
            var queryString = $"SELECT * FROM {sourceName}";
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand(queryString, connection);
            var dataReader = command.ExecuteReader();
            var result = new List<TModel>();

            while (dataReader.Read())
                result.Add(MapRecordToModel(dataReader));

            command.Dispose();
            dataReader.Close();
            connection.Close();
            return result;
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

        protected TModel MapRecordToModel(IDataRecord record)
        {
            var result = new TModel();
            var properties = result.GetType().GetProperties();

            foreach (var propery in properties)
            {
                propery.SetValue(result, record.GetValue(record.GetOrdinal(propery.Name)));
            }

            return result;
        }

        private static void InitTypeToSqlDbType()
        {
            typeToSqlDbType = new Dictionary<Type, SqlDbType>();
            typeToSqlDbType.Add(typeof(Int64), SqlDbType.BigInt);
            typeToSqlDbType.Add(typeof(Int32), SqlDbType.Int); // This is also int
            typeToSqlDbType.Add(typeof(Int16), SqlDbType.SmallInt);
            typeToSqlDbType.Add(typeof(string), SqlDbType.VarChar);
        }
    }
}
