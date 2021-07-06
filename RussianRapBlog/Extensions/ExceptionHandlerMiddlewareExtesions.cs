using Microsoft.AspNetCore.Builder;
using RussianRapBlog.Middlewares;

namespace RussianRapBlog.Extensions
{
    /// <summary>
    ///     расширение для подключения ExceptionHandlerMiddleware
    /// </summary>
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}