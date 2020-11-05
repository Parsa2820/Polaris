using Database.Exceptions.MicrosoftSqlServer;
using Database.Filtering.Filter;
using Database.Validation.MicrosoftSqlServer;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.Communication.MicrosoftSqlServer
{
    public class MSqlHandler<TModel> : IDatabaseHandler<TModel>
        where TModel : new()
    {
        protected static Dictionary<Type, SqlDbType> typeToSqlDbType;
        protected static string tableColumns;
        protected string connectionString;
        private static Dictionary<string, string> modelFilterMapping;
        private const int MaxLenghtDbArray = -1;

        public MSqlHandler()
        {
            connectionString = MSqlClientFactory.Instance.GetClient();
            InitTypeToSqlDbType();
            InitTableColumns();
            InitModelFilterMapping();
        }

        public void BulkInsert(IEnumerable<TModel> models, string sourceName)
        {
            CheckSource(sourceName, true);
            using var dataTable = new DataTable();
            var properties = models.First().GetType().GetProperties();

            foreach (var property in properties)
                dataTable.Columns.Add(new DataColumn(property.Name, property.PropertyType));

            foreach (var model in models)
            {
                var dataRow = dataTable.NewRow();

                foreach (var property in properties)
                    dataRow[property.Name] = property.GetValue(model);

                dataTable.Rows.Add(dataRow);
            }

            using var connection = new SqlConnection(connectionString);
            using var sqlBulk = new SqlBulkCopy(connection)
            {
                DestinationTableName = sourceName
            };

            foreach (var property in properties)
                sqlBulk.ColumnMappings.Add(property.Name, property.Name);

            connection.Open();
            sqlBulk.WriteToServer(dataTable);

        }

        public IEnumerable<TModel> FetchAll(string sourceName)
        {
            var queryString = $"SELECT {tableColumns} FROM {sourceName}";
            using var command = new SqlCommand(queryString);
            return FetchByCommand(command);
        }

        protected List<TModel> FetchByCommand(SqlCommand command)
        {
            var result = new List<TModel>();
            using var connection = new SqlConnection(connectionString);
            command.Connection = connection;
            connection.Open();
            using var dataReader = command.ExecuteReader();

            while (dataReader.Read())
                result.Add(MapRecordToModel(dataReader));

            return result;
        }

        public void Insert(TModel model, string sourceName)
        {
            BulkInsert(new List<TModel> { model }, sourceName);
        }

        public void CheckSource(string sourceName, bool recreate = false)
        {
            try
            {
                MicrosoftSqlTableValidator.ValidateTable(sourceName);
            }
            catch (InvalidSqlTableException e)
            {
                if (recreate)
                {
                    CreateNewTable(sourceName);
                }
                else
                    throw e;
            }
        }

        private void CreateNewTable(string sourceName)
        {
            var properties = typeof(TModel).GetProperties();
            var columns = new StringBuilder();

            foreach (var property in properties)
            {
                columns.Append($"{property.Name} {typeToSqlDbType[property.PropertyType]}");

                if (typeToSqlDbType[property.PropertyType] == SqlDbType.NVarChar)
                    columns.Append("(" + (MaxLenghtDbArray == -1 ? "MAX" : MaxLenghtDbArray.ToString()) + ")");

                columns.AppendLine(", ");
            }

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand($"CREATE TABLE {sourceName} ({columns})", connection);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public IEnumerable<TModel> RetrieveQueryDocumentsByFilter(string[] container, string sourceName, Pagination pagination = null)
        {
            var commandString = new SqlServerFilter(container, modelFilterMapping, sourceName, tableColumns).Interpret();
            Console.WriteLine(commandString);
            return FetchByCommand(new SqlCommand(commandString));
        }

        protected TModel MapRecordToModel(IDataRecord record)
        {
            var result = new TModel();
            var properties = result.GetType().GetProperties();

            foreach (var propery in properties)
                propery.SetValue(result, record.GetValue(record.GetOrdinal(propery.Name)));

            return result;
        }

        private static void InitTypeToSqlDbType()
        {
            typeToSqlDbType = new Dictionary<Type, SqlDbType>
            {
                { typeof(long), SqlDbType.BigInt }, // This is also Int64
                { typeof(int), SqlDbType.Int }, // This is also Int32
                { typeof(short), SqlDbType.SmallInt }, // This is also Int16
                { typeof(string), SqlDbType.NVarChar }
            };
        }

        private void InitTableColumns() => tableColumns = string.Join(", ", typeof(TModel).GetProperties().Select(x => x.Name));

        private void InitModelFilterMapping()
        {
            var properties = typeof(TModel).GetProperties();
            modelFilterMapping = new Dictionary<string, string>();

            foreach (var property in properties)
                modelFilterMapping.Add(property.Name.ToLower(), GetFilterType(property.PropertyType));
        }

        private string GetFilterType(Type type)
        {
            if (type.Equals(typeof(int)) || type.Equals(typeof(long)) || type.Equals(typeof(short)))
                return "numeric";
            else
                return "text";
        }
    }
}
