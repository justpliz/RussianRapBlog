using System;

namespace Models.Exceptions
{
    /// <summary>
    ///     Исключение в бизнес логике
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}