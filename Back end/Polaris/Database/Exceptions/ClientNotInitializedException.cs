using System;

namespace Database.Exceptions
{
    public class ClientNotInitializedException : Exception
    {
        public ClientNotInitializedException() :
            base("Initialize elastic client before using it.")
        {
        }
    }
}