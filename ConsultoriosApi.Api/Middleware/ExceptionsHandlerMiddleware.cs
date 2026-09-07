using ConsultoriosApi.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace ConsultoriosApi.Api.Middleware
{
    public class ExceptionsHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionsHandlerMiddleware(RequestDelegate next )
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next( context );
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }
        private Task HandleException(HttpContext context, Exception exception)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var result = string.Empty;

            switch (exception)
            {
                case NotFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    break;
                case ValidationException validationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.ValidationErrors);
                    break;
            }
            context.Response.StatusCode = (int)httpStatusCode;
            return context.Response.WriteAsync(result);

        }

    }
    public static class ExceptionsHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsHandlerMiddleware>();
        }
    }
}
