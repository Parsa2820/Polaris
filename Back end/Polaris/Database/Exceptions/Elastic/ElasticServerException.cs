using System;

namespace Database.Exceptions.Elastic
{
    public class ElasticServerException : Exception
    {
        public ElasticServerException(string message) : base(message)
        {
        }
    }
}