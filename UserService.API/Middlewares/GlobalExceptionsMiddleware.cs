using System.Net;
using System.Text.Json;
using UserService.Core.Exceptions;

namespace UserService.API.Middlewares
{
    public class GlobalExceptionsMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (UserServiceException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, UserServiceException ex)
        {
            var code = HttpStatusCode.InternalServerError;

            switch (ex.Type)
            {
                case UserServiceErrorTypes.ENTITY_NOT_FOUND:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            var result = JsonSerializer.Serialize(new { ex.Message, code });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new { ex.Message, code });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
