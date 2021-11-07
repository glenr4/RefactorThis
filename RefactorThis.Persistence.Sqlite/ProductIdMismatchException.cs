using System;

namespace RefactorThis.Persistence.Sqlite
{
    public class ProductIdMismatchException : ArgumentException
    {
        public ProductIdMismatchException(string message) : base(message)
        {
        }
    }
}