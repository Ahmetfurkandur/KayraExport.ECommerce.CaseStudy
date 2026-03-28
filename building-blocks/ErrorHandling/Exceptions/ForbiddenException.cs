namespace ErrorHandling.Exceptions
{
    public class ForbiddenException : BaseException
    {

        public ForbiddenException()
        { }

        public ForbiddenException(string message, ICollection<ErrorDetail> errors) : base(message, errors)
        {
        }
    }
}
