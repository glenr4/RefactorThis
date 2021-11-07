using System.Collections.Generic;

namespace RefactorThis.Domain.Exceptions
{
    public class ProductNotFoundException : KeyNotFoundException
    {
        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}