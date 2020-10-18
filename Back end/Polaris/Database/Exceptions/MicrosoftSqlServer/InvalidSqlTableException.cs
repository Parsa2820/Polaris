using System;

namespace Database.Exceptions.MicrosoftSqlServer
{
    class InvalidSqlTableException : Exception
    {
        public InvalidSqlTableException(string message) : base(message)
        {
        }
    }
}
