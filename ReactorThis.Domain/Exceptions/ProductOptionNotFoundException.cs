using System.Collections.Generic;

namespace RefactorThis.Domain.Exceptions
{
    public class ProductOptionNotFoundException : KeyNotFoundException
    {
        public ProductOptionNotFoundException(string message) : base(message)
        {
        }
    }
}