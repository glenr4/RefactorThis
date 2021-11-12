using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RefactorThis.API.Exceptions;
using RefactorThis.Domain.Exceptions;
using Serilog;
using System.Net;

namespace RefactorThis.API
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UnhandledExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.Error("{ex}", context.Exception);

            var exceptionType = context.Exception.GetType().Name;
            int statusCode = 0;
            string message = context.Exception.Message;

            switch (exceptionType)
            {
                // Custom exceptions with messages
                case nameof(ProductNotFoundException):
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;

                case nameof(ProductIdMismatchException):
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case nameof(DbUpdateException):
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                // System exceptions
                case nameof(DbUpdateConcurrencyException):
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = "Item not found";
                    break;

                default:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "An error occurred";
                    break;
            }

            var result = new ObjectResult(new
            {
                message
            })
            {
                StatusCode = statusCode
            };

            context.Result = result;
        }
    }
}