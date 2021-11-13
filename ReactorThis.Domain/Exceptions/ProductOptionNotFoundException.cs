using System.Collections.Generic;

namespace RefactorThis.Domain.Exceptions
{
    public class ProductOptionNotFoundException : KeyNotFoundException
    {
        /// <summary>
        /// Usage: string.Format(ProductOptionNotFoundException.MessageTemplate, param);
        /// </summary>
        public static string MessageTemplate { get; } = "Product Option with Id: {0} and Product Id: {1} does not exist";

        public ProductOptionNotFoundException(string message) : base(message)
        {
        }
    }
}