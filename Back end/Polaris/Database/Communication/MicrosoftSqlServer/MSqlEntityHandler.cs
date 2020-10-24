using Database.Exceptions;
using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Database.Communication.MicrosoftSqlServer
{
    public class MSqlEntityHandler<TModel, TType> : MSqlHandler<TModel>, IEntityHandler<TModel, TType>
    where TModel : Entity<TType>, new()
    {
        public void DeleteEntity(TType id, string sourceName)
        {
            var querryString = $"DELETE {sourceName} WHERE Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(querryString, connection))
                {
                    command.Parameters.Add("@Id", typeToSqlDbType[typeof(TType)]);
                    command.Parameters["@Id"].Value = id;
                    command.ExecuteNonQuery();

                }
            }
        }


        public IEnumerable<TModel> GetEntities(TType[] ids, string sourceName)
        {
            var idsList = ids.ToList();
            var result = new List<TModel>();

            using (var command = new SqlCommand())
            {
                var parameters = new string[idsList.Count()];

                for (int i = 0; i < idsList.Count(); i++)
                {
                    parameters[i] = $"@Id{i + 1}";
                    command.Parameters.AddWithValue(parameters[i], idsList[i]);
                }

                command.CommandText = $"SELECT * FROM {sourceName} WHERE Id IN ({string.Join(", ", parameters)})";
                using (var connection = new SqlConnection(connectionString))
                {
                    command.Connection = connection;
                    using (var dataReader = command.ExecuteReader())
                    {

                        while (dataReader.Read())
                            result.Add(MapRecordToModel(dataReader));
                    }
                }
            }
            return result;
        }

        public TModel GetEntity(TType id, string sourceName)
        {
            TModel result;
            var queryString = $"SELECT * FROM {sourceName} WHERE Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@Id", typeToSqlDbType[typeof(TType)]);
                    command.Parameters["@Id"].Value = id;
                    using (var dataReader = command.ExecuteReader())
                    {

                        if (!dataReader.Read())
                            throw new EntityNotFoundException($"Entity with id: \"{id}\" not found in table \"{sourceName}\"");

                        result = MapRecordToModel(dataReader);
                    }
                }

            }
            return result;
        }

        public void UpdateEntity(TModel newEntity, string sourceName)
        {
            DeleteEntity(newEntity.Id, sourceName);
            Insert(newEntity, sourceName);
        }
    }
}
