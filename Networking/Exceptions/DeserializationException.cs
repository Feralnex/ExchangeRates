using System;

namespace Networking.Exceptions
{
    class DeserializationException : Exception
    {
        public DeserializationException(string message) : base(message) { }
    }
}
