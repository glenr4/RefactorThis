using System;

namespace RefactorThis.Persistence.Sqlite.Exceptions
{
    public class ProductIdMismatchException : ArgumentException
    {
        public ProductIdMismatchException(string message) : base(message)
        {
        }
    }
}