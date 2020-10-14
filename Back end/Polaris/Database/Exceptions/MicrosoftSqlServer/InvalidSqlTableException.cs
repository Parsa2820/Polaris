using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Exceptions.MicrosoftSqlServer
{
    class InvalidSqlTableException : Exception
    {
        public InvalidSqlTableException(string message) : base(message)
        {
        }
    }
}
