using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RefactorThis.API.Logging
{
    public class RequestLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        private readonly string _messageTemplate = "{requestMethod} {requestPath}{queryString} from {RemoteIP} => {statusCode} in {responseTime:0.000} ms TraceId: {TraceId}";

        public RequestLogger(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sw = Stopwatch.StartNew();

            // Add ASPNET trace ID as a property on all logged messages
            using (LogContext.PushProperty("TraceID", httpContext.TraceIdentifier))
            {
                try
                {
                    await _next(httpContext);

                    sw.Stop();

                    int? statusCode = httpContext.Response?.StatusCode;
                    var level = statusCode >= 500 ? LogEventLevel.Error
                        : statusCode >= 400 ? LogEventLevel.Warning
                        : LogEventLevel.Debug;

                    _logger.Write(level, _messageTemplate, httpContext.Request.Method, httpContext.Request.Path, httpContext.Request.QueryString, httpContext.Connection.RemoteIpAddress, statusCode, sw.Elapsed.TotalMilliseconds);
                }
                catch (Exception ex)
                {
                    sw.Stop();

                    _logger.Error(nameof(RequestLogger), ex);
                }
            }
        }
    }
}