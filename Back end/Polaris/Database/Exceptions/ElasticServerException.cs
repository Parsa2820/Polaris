using System;

namespace Database.Exceptions
{
    public class ElasticServerException : Exception
    {
        public ElasticServerException(string message) : base(message)
        {
        }
    }
}