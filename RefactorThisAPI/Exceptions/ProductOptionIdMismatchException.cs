using System;

namespace RefactorThis.API.Exceptions
{
    public class ProductOptionIdMismatchException : ArgumentException
    {
        /// <summary>
        /// Usage: string.Format(ProductOptionIdMismatchException.MessageTemplate, param);
        /// </summary>
        public static string MessageTemplate { get; } = "ProductOptionId does not match in URL and Object: {0}";

        public ProductOptionIdMismatchException(string message) : base(message)
        {
        }
    }
}