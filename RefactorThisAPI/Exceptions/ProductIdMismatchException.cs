using System;

namespace RefactorThis.API.Exceptions
{
    public class ProductIdMismatchException : ArgumentException
    {
        public ProductIdMismatchException(string message) : base(message)
        {
        }
    }
}