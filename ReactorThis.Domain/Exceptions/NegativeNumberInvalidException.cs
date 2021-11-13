using System;

namespace RefactorThis.Domain.Exceptions
{
    public class NegativeNumberInvalidException : Exception
    {
        /// <summary>
        /// Usage: string.Format(NegativeNumberInvalidException.MessageTemplate, param);
        /// </summary>
        public static string MessageTemplate { get; } = "Argument {0} cannot be negative";

        public NegativeNumberInvalidException(string message) : base(message)
        {
        }
    }
}