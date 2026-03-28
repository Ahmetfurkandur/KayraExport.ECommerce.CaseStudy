namespace ErrorHandling.Exceptions
{
    //HTTP 400 için kullanacağımız exception sınıfı
    public class BadRequestException : BaseException
    {

        public BadRequestException()
        { }

        public BadRequestException(string message, ICollection<ErrorDetail> errors) : base(message, errors)
        {
        }
    }
}
