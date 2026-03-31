// Middleware/ErrorHandlingMiddleware.cs
using System.Text;
using System.Text.Json;
using MovieAPI.Logging;

namespace MovieAPI.Middleware //тут я отлавливаю ошибки
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Записываем ошибку в файл
                ErrorLogger.LogError($"Request: {context.Request.Method} {context.Request.Path}", ex);
            }
        }
    }
}