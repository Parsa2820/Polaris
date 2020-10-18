using Dapper;
using Database.Communication.MicrosoftSqlServer;
using Database.Exceptions.MicrosoftSqlServer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Database.Validation.MicrosoftSqlServer
{
    class MicrosoftSqlTableValidator
    {
        public static void ValidateTable(string sourceName)
        {
            var queryString = $"SELECT NULL FROM {sourceName}";
            var connectionString = MSqlClientFactory.GetInstance().GetClient();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Execute(queryString);
                    return;
                }
                catch (Exception e)
                {
                    throw new InvalidSqlTableException($"\"{sourceName}\" table does not exist");
                }
            }
        }
    }
}
