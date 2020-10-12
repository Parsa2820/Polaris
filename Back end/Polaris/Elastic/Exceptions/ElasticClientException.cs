using System;

namespace Database.Exceptions
{
    public class ElasticClientException : Exception
    {
        public ElasticClientException(string message) : base(message)
        {
        }
    }
}