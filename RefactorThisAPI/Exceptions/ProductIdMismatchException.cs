using System;

namespace RefactorThis.API.Exceptions
{
    public class ProductIdMismatchException : ArgumentException
    {
        /// <summary>
        /// Usage: string.Format(ProductIdMismatchException.MessageTemplate, param);
        /// </summary>
        public static string MessageTemplate { get; } = "ProductId does not match in URL and Object: {0}";

        public ProductIdMismatchException(string message) : base(message)
        {
        }
    }
}