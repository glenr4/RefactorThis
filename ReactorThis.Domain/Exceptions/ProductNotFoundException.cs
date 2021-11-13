using System.Collections.Generic;

namespace RefactorThis.Domain.Exceptions
{
    public class ProductNotFoundException : KeyNotFoundException
    {
        /// <summary>
        /// Usage: string.Format(ProductNotFoundException.MessageTemplate, param);
        /// </summary>
        public static string MessageTemplate { get; } = "Product with Id: {0} was not found";

        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}