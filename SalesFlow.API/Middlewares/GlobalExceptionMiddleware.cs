using FluentValidation;
using SalesFlow.Core.Exceptions;
using SalesFlow.Core.Results;
using System.Net;
using System.Text.Json;

namespace SalesFlow.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            Result result;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;

                    result = Result.Failure(
                        "Validation failed.",
                        validationException.Errors
                            .Select(x => x.ErrorMessage)
                            .ToList());

                    break;

                case BusinessException businessException:
                    statusCode = HttpStatusCode.BadRequest;

                    result = Result.Failure(
                        businessException.Message);

                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;

                    result = Result.Failure(
                        notFoundException.Message);

                    break;

                case ForbiddenException forbiddenException:
                    statusCode = HttpStatusCode.Forbidden;

                    result = Result.Failure(
                        forbiddenException.Message);

                    break;

                default:
                    statusCode =
                        HttpStatusCode.InternalServerError;

                    result = Result.Failure(
                        "An unexpected error occurred.");

                    break;
            }

            context.Response.StatusCode =
                (int)statusCode;

            var json = JsonSerializer.Serialize(
                result,
                JsonOptions);

            await context.Response.WriteAsync(json);
        }
    }
}