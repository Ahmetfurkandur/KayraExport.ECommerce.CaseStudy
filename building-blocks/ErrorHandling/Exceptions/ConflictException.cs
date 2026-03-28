namespace ErrorHandling.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException()
        { }

        public ConflictException(string message, ICollection<ErrorDetail> errors) : base(message, errors)
        {
        }
    }
}
