using BLL.DTOs;
using BLL.Services;
using Serilog;

namespace Ipms.backend.CustomMiddleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context,IErrorLogService errorLogService)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var requestPath = context.Request.Path;
                var httpMethod = context.Request.Method;
                var userName = context.User?.Identity?.Name ?? "Anonymous";

                var statusCode = ex switch
                {
                    ArgumentException => StatusCodes.Status400BadRequest,
                    InvalidOperationException => StatusCodes.Status400BadRequest,

                    KeyNotFoundException => StatusCodes.Status404NotFound,

                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

                    TimeoutException => StatusCodes.Status408RequestTimeout,

                    NotImplementedException => StatusCodes.Status501NotImplemented,

                    _ => StatusCodes.Status500InternalServerError
                };

                Log.Error(ex,
                    "Error at {Path} with {Method} by {User}",
                    requestPath,
                    httpMethod,
                    userName);

                var error = new ErrorLogDTO
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    LoggedDate = DateTime.Now,
                    RequestPath = requestPath,
                    HttpMethod = httpMethod,
                    UserName = userName,
                    StatusCode = statusCode
                };

                try
                {
                    await errorLogService.ExceptionLog(error);
                }
                catch (Exception logEx)
                {
                    Log.Error(logEx, "Failed to log error to database");
                }

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(
                        "{\"message\":\"An unexpected error occurred. Please try again later.\"}");
                }
            }
        }
    }
}
