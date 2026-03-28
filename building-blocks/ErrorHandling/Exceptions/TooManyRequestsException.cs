namespace ErrorHandling.Exceptions
{
    public class TooManyRequestsException : BaseException
    {
        public TooManyRequestsException()
        { }

        public TooManyRequestsException(string message, ICollection<ErrorDetail> errors) : base(message, errors)
        {
        }
    }
}
