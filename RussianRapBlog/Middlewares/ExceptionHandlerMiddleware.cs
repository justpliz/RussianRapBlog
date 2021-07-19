using System;
using System.Net;
using System.Threading.Tasks;
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

            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                await HandleExceptionMessageAsync(context, ex.Message, HttpStatusCode.NotFound)
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
            context.Response.StatusCode = (int) statusCode;
            return context.Response.WriteAsJsonAsync(message);
        }
    }
}
