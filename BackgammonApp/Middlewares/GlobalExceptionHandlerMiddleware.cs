using Common.Enums;
using Common.Exceptions;
using FluentValidation;
using System.Net;
using ApplicationException = Common.Exceptions.ApplicationException;

namespace WebAPI.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            object responseBody;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;

                    var errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    responseBody = new
                    {
                        Type = "ValidationFailure",
                        Code = FunctionCode.ValidationError,
                        Message = "One or more validation errors occurred.",
                        Errors = errors
                    };
                    break;

                case ValidationFailedException customValidationException:
                    statusCode = HttpStatusCode.BadRequest;

                    responseBody = new
                    {
                        Type = "ValidationFailure",
                        Code = FunctionCode.ValidationError,
                        Message = customValidationException.Message,
                        Errors = customValidationException.Errors
                    };
                    break;

                case ApplicationException appException:
                    statusCode = appException switch
                    {
                        NotFoundException => HttpStatusCode.NotFound,
                        ForbiddenException => HttpStatusCode.Forbidden,
                        BusinessRuleException => HttpStatusCode.Conflict,
                        _ => HttpStatusCode.InternalServerError
                    };

                    responseBody = new
                    {
                        Type = appException.GetType().Name.Replace("Exception", ""),
                        Code = appException.ErrorCode,
                        Message = appException.Message
                    };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;

                    responseBody = new
                    {
                        Type = "InternalServerError",
                        Code = FunctionCode.InternalError,
                        Message = context.RequestServices
                            .GetRequiredService<IWebHostEnvironment>()
                            .IsDevelopment()
                                ? exception.ToString()
                                : "An unexpected server error occurred."
                    };
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsJsonAsync(responseBody);
        }
    }
}
