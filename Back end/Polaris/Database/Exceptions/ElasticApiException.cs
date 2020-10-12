using System;

namespace Database.Exceptions
{
    public class ElasticApiException : Exception
    {
        public ElasticApiException(string message) : base(message)
        {
        }
    }
}