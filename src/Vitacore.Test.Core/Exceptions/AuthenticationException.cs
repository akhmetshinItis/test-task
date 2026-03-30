namespace Vitacore.Test.Core.Exceptions
{
    /// <summary>
    /// Исключение об ошибке аутентификации
    /// </summary>
    public class AuthenticationException : AppException
    {
        public AuthenticationException(string message) : base(message)
        {
        }
    }
}