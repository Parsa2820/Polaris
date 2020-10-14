using System;

namespace Database.Exceptions.Elastic
{
    public class ElasticApiException : Exception
    {
        public ElasticApiException(string message) : base(message)
        {
        }
    }
}