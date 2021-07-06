using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models.Exceptions;

namespace RussianRapBlog.Middlewares
{
    /// <summary>
    ///     Middleware для глобального отлова исключений
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;


        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }

            catch (BusinessException ex)
            {
                _logger.LogError(ex.Message);
                await HandleExceptionMessageAsync(context, ex.Message, HttpStatusCode.InternalServerError)
                    .ConfigureAwait(false);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await HandleExceptionMessageAsync(context, "Неизвестная ошибка сервера",
                    HttpStatusCode.InternalServerError).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, string message, HttpStatusCode statusCode)
        {
            var result = new HttpStatusCodeResult(statusCode, message);
            return context.Response.WriteAsJsonAsync(result);
        }
    }
}
