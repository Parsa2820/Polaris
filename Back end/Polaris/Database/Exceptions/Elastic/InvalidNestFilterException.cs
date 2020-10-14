using System;

namespace Database.Exceptions.Elastic
{
    public class InvalidNestFilterException : Exception
    {
        public InvalidNestFilterException(string message) : base(message)
        {
        }
    }
}