using System;

namespace RefactorThis.Domain.Exceptions
{
    public class EmptyArgumentException : ArgumentException
    {
        /// <summary>
        /// Usage: string.Format(EmptyArgumentException.MessageTemplate, param);
        /// </summary>
        public static string MessageTemplate { get; } = "Argument {0} cannot be empty or null";

        public EmptyArgumentException(string message) : base(message)
        {
        }
    }
}