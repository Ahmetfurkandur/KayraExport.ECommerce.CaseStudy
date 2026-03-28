using ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ErrorHandling.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {

        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate _next)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode code;
            LogLevel logLevel;
            string errorCode;

            switch (ex)
            {
                // Custom exceptions
                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    logLevel = LogLevel.Warning;
                    errorCode = "BAD_REQUEST";
                    break;

                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    logLevel = LogLevel.Warning;
                    errorCode = "NOT_FOUND";
                    break;

                case ConflictException:
                    code = HttpStatusCode.Conflict;
                    logLevel = LogLevel.Warning;
                    errorCode = "CONFLICT";
                    break;

                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    logLevel = LogLevel.Warning;
                    errorCode = "UNAUTHORIZED";
                    break;

                // Built-in / Infrastructure exceptions
                case OperationCanceledException:
                    code = HttpStatusCode.BadRequest;
                    logLevel = LogLevel.Information;
                    errorCode = "REQUEST_CANCELLED";
                    break;

                case TimeoutException:
                    code = HttpStatusCode.GatewayTimeout;
                    logLevel = LogLevel.Error;
                    errorCode = "TIMEOUT";
                    break;

                case InvalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    logLevel = LogLevel.Error;
                    errorCode = "INVALID_OPERATION";
                    break;

                // string match gereken tipler (farklı assembly'den gelenler)
                case var e when e.GetType().Name == "ValidationException":
                    code = HttpStatusCode.BadRequest;
                    logLevel = LogLevel.Warning;
                    errorCode = "VALIDATION_ERROR";
                    break;

                case var e when e.GetType().Name == "SqlException":
                    code = HttpStatusCode.InternalServerError;
                    logLevel = LogLevel.Critical;
                    errorCode = "DB_ERROR";
                    break;

                case var e when e.GetType().Name == "BadHttpRequestException":
                    code = HttpStatusCode.BadRequest;
                    logLevel = LogLevel.Warning;
                    errorCode = "BAD_HTTP_REQUEST";
                    break;

                case var e when e.GetType().Name == "RedisConnectionException":
                    code = HttpStatusCode.InternalServerError;
                    logLevel = LogLevel.Critical;
                    errorCode = "CACHE_ERROR";
                    break;

                // Fallback
                default:
                    code = HttpStatusCode.InternalServerError;
                    logLevel = LogLevel.Error;
                    errorCode = "INTERNAL_SERVER_ERROR";
                    break;
            }

            // Structured logging — Seq'te filtrelenebilir alanlar
            logger.Log(logLevel,
                ex,
                "Exception on {Method} {Path} | {ErrorCode} | {StatusCode} | {ExceptionType}",
                context.Request.Method,
                context.Request.Path,
                errorCode,
                (int)code,
                ex.GetType().Name);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var errorDetails = ex is BaseException baseEx
                ? baseEx.Errors
                : new List<ErrorDetail>
                {
            new() { Code = (int)code, Description = ex.Message, Title = ex.GetType().Name }
                };

            var result = new { Message = errorCode, Errors = errorDetails };

            await context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
