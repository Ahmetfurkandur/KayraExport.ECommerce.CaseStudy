using ErrorHandling;
using LogService.Application.Interfaces;
using LogService.Domain.Entities;
using MediatR;

namespace LogService.Application.Features.Events.Queries.GetEventByIdQuery
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQueryRequest, LogEntry>
    {

        private readonly ILogQueryRepository logQueryRepository;

        public GetEventByIdQueryHandler(ILogQueryRepository logQueryRepository)
        {
            this.logQueryRepository = logQueryRepository;
        }

        public async Task<LogEntry> Handle(GetEventByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var log = await logQueryRepository.GetLogByIdAsync(request.EventId);

            ErrorBuilder.Create(404)
                    .WithTitle("Kayıt Bulunamadı")
                    .WithDescription("Log kaydı bulunamadı. Lütfen Daha sonra tekrar deneyiniz.")
                    .ThrowIfNull(log);

            return log;
        }
    }
}
