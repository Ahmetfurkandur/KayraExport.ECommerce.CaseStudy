namespace ErrorHandling.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException()
        { }
        public NotFoundException(string message, ICollection<ErrorDetail> errors) : base(message, errors)
        {
        }
    }
}
