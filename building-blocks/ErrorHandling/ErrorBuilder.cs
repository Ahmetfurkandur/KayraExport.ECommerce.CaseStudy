using ErrorHandling.Exceptions;

namespace ErrorHandling
{
    /// <summary>
    /// Daha esnek bir şekilde Exception fırlatabilmek için Builder Design Pattern kullanan sınıftır.
    /// </summary>
    public class ErrorBuilder
    {
        private readonly ErrorDetail errorDetail;

        public ErrorBuilder(int code)
        {
            errorDetail = new ErrorDetail() { Code = code };
        }

        public static ErrorBuilder Create(int code) => new(code);

        public ErrorBuilder WithTitle(string title)
        {
            errorDetail.Title = title;
            return this;
        }

        public ErrorBuilder WithDescription(string description)
        {
            errorDetail.Description = description;
            return this;
        }

        public ErrorBuilder Throw()
        {
            return errorDetail.Code switch
            {
                400 => throw new BadRequestException("HATALI ISTEK", [errorDetail]),
                401 => throw new UnauthorizedException("YETKISIZ ERISIM", [errorDetail]),
                403 => throw new ForbiddenException("ERISIM ENGELLENDI", [errorDetail]),
                404 => throw new NotFoundException("KAYIT BULUNAMADI", [errorDetail]),
                409 => throw new ConflictException("CATISMA HATASI", [errorDetail]),
                429 => throw new TooManyRequestsException("ISTEK LIMITI ASILDI", [errorDetail]),
                _ => throw new BadRequestException("HATALI ISTEK", [errorDetail])
            };
        }
        public ErrorBuilder ThrowIf(bool condition) //koşula göre fırlatmak için 
        {
            if (condition)
            {
                return Throw();
            }
            return this;
        }

        public ErrorBuilder ThrowIfNull(object condition) // null ise fırlatmak için
        {
            if (condition is null)
            {
                return Throw();
            }
            return this;
        }
    }
}
