using System;

namespace RefactorThis.Domain.Exceptions
{
    public class NegativeNumberInvalidException : Exception
    {
        public NegativeNumberInvalidException(string message) : base(message)
        {
        }
    }
}