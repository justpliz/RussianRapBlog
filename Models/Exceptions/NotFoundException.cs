using System;

namespace Models.Exceptions
{
    /// <summary>
    ///     Исключение "не найдено"
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}