namespace ErrorHandling.Exceptions
{
    public class BaseException : Exception
    {
        public readonly ICollection<ErrorDetail> Errors;
        public BaseException() { }
        public BaseException(string message) : base(message)
        { }
        public BaseException(string message, ICollection<ErrorDetail> errors) : base(message)
        {
            Errors = errors ?? new List<ErrorDetail>();
        }
    }
}
