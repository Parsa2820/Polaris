using Database.Communication.MicrosoftSqlServer;
using Database.Exceptions.MicrosoftSqlServer;
using System;
using System.Data.SqlClient;

namespace Database.Validation.MicrosoftSqlServer
{
    class MicrosoftSqlTableValidator
    {
        public static void ValidateTable(string sourceName)
        {
            var queryString = $"SELECT NULL FROM {sourceName}";
            var connectionString = MSqlClientFactory.Instance.GetClient();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.ExecuteReader();
                return;
            }
            catch (Exception)
            {
                throw new InvalidSqlTableException($"\"{sourceName}\" table does not exist");
                connection.Open();
                try
                {
                    using (var command = new SqlCommand(queryString, connection))
                    {
                        command.ExecuteReader();
                    }
                }
                catch (Exception)
                {
                    throw new InvalidSqlTableException($"\"{sourceName}\" table does not exist");
                }
            }
        }
    }
}
