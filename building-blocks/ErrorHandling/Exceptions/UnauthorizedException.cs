namespace ErrorHandling.Exceptions
{
    public class UnauthorizedException : BaseException
    {

        public UnauthorizedException()
        { }

        public UnauthorizedException(string message, ICollection<ErrorDetail> errors) : base(message, errors)
        {
        }
    }
}
