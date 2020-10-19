using Database.Exceptions;
using Models;
using Nest;
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

        public TModel GetEntity(TType id, string sourceName)
        {
            var queryString = $"SELECT * FROM {sourceName} WHERE Id = {id}";
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand(queryString, connection);
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
            throw new NotImplementedException();
        }
    }
}
