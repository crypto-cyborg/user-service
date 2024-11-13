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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            if (ex is UserServiceException usex)
            {
                code = usex.Type switch 
                {
                    UserServiceErrorTypes.ENTITY_NOT_FOUND => HttpStatusCode.NotFound,
                    UserServiceErrorTypes.INVALID_USERNAME => HttpStatusCode.Forbidden,
                    UserServiceErrorTypes.INVALID_EMAIL => HttpStatusCode.Forbidden,
                    _ => code
                };
            }

            var result = JsonSerializer.Serialize(new { ex.Message, code });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
