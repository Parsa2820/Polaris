using Database.Exceptions;
using Models;
using System.Collections.Generic;
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
            var querryString = $"DELETE {sourceName} WHERE Id = @Id";
            var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(querryString, connection);
            command.Parameters.Add("@Id", typeToSqlDbType[typeof(TType)]);
            command.Parameters["@Id"].Value = id;
            connection.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }

        public IEnumerable<TModel> GetEntities(TType[] ids, string sourceName)
        {
            var idsList = ids.ToList();
            var command = new SqlCommand();
            var parameters = new string[idsList.Count()];
            
            for (int i = 0; i < idsList.Count(); i++)
            {
                parameters[i] = $"@Id{i + 1}";
                command.Parameters.AddWithValue(parameters[i], idsList[i]);
            }

            command.CommandText = $"SELECT * FROM {sourceName} WHERE Id IN ({string.Join(", ", parameters)})";
            var connection = new SqlConnection(connectionString);
            command.Connection = connection;
            connection.Open();
            var dataReader = command.ExecuteReader();
            var result = new List<TModel>();

            while (dataReader.Read())
                result.Add(MapRecordToModel(dataReader));

            command.Dispose();
            dataReader.Close();
            connection.Close();
            return result;
        }

        public TModel GetEntity(TType id, string sourceName)
        {
            var queryString = $"SELECT * FROM {sourceName} WHERE Id = @Id";
            var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(queryString, connection);
            command.Parameters.Add("@Id", typeToSqlDbType[typeof(TType)]);
            command.Parameters["@Id"].Value = id;
            connection.Open();
            var dataReader = command.ExecuteReader();

            if (!dataReader.Read())
                throw new EntityNotFoundException($"Entity with id: \"{id}\" not found in table \"{sourceName}\"");

            var result = MapRecordToModel(dataReader);
            command.Dispose();
            dataReader.Close();
            connection.Close();
            return result;
        }

        public void UpdateEntity(TModel newEntity, string sourceName)
        {
            DeleteEntity(newEntity.Id, sourceName);
            Insert(newEntity, sourceName);
        }
    }
}
