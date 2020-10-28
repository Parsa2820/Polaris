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
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(querryString, connection);
            command.Parameters.Add("@Id", typeToSqlDbType[typeof(TType)]);
            command.Parameters["@Id"].Value = id;
            connection.Open();
            command.ExecuteNonQuery();
        }


        public IEnumerable<TModel> GetEntities(TType[] ids, string sourceName)
        {
            var idsList = ids.ToList();
            using var command = new SqlCommand();
            var parameters = new string[idsList.Count()];

            for (int i = 0; i < idsList.Count(); i++)
            {
                parameters[i] = $"@Id{i + 1}";
                command.Parameters.AddWithValue(parameters[i], idsList[i]);
            }

            command.CommandText = $"SELECT {tableColumns} FROM {sourceName} WHERE Id IN ({string.Join(", ", parameters)})";

            return FetchByCommand(command);
        }

        public TModel GetEntity(TType id, string sourceName)
        {
            var result = GetEntities(new TType[] { id }, sourceName);

            if (!result.Any())
                throw new EntityNotFoundException($"Entity with id: \"{id}\" not found in table \"{sourceName}\"");

            return result.First();
        }

        public void UpdateEntity(TModel newEntity, string sourceName)
        {
            var properties = typeof(TModel).GetProperties();
            var setter = new string[properties.Count()];
            using var command = new SqlCommand();
            command.Parameters.AddWithValue("@Id", newEntity.Id);

            for (int i = 0; i < properties.Count(); i++)
            {
                if (properties[i].Name.Equals("Id"))
                    continue;

                setter[i] = $"{properties[i].Name} = @{properties[i].Name}";
                command.Parameters.AddWithValue($"@{properties[i].Name}", properties[i].GetValue(newEntity));
            }

            using var connection = new SqlConnection(connectionString);
            command.Connection = connection;
            command.CommandText = $"UPDATE {sourceName} SET {string.Join(", ", setter)} WHERE Id = @Id";
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
