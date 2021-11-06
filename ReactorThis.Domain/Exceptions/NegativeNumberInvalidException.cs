using System;

namespace RefactorThis.Domain.Exceptions
{
    internal class NegativeNumberInvalidException : Exception
    {
        public NegativeNumberInvalidException(string message) : base(message)
        {
        }
    }
}