using System;

namespace Database.Exceptions.Elastic
{
    public class InvalidElasticIndexException : Exception
    {
        public InvalidElasticIndexException(string message) : base(message)
        {
        }
    }
}