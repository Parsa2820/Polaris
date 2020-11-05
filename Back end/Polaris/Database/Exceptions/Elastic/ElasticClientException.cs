using System;

namespace Database.Exceptions.Elastic
{
    public class ElasticClientException : Exception
    {
        public ElasticClientException(string message) : base(message)
        {
        }
    }
}