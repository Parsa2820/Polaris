using System;

namespace Database.Exceptions
{
    public class InvalidNestFilterException : Exception
    {
        public InvalidNestFilterException(string message) : base(message)
        {
        }
    }
}