using System;

namespace Database.Exceptions
{
    public class InvalidElasticIndexException : Exception
    {
        public InvalidElasticIndexException(string message) : base(message)
        {
        }
    }
}