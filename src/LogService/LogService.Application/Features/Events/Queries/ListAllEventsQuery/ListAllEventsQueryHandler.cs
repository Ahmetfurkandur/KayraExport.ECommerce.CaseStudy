using ErrorHandling;
using LogService.Application.Interfaces;
using LogService.Domain.Entities;
using MediatR;

namespace LogService.Application.Features.Events.Queries.ListAllEventsQuery
{
    public class ListAllEventsQueryHandler : IRequestHandler<ListAllEventsQueryRequest, IReadOnlyList<LogEntry>>
    {
        private readonly ILogQueryRepository logQueryRepository;

        public ListAllEventsQueryHandler(ILogQueryRepository logQueryRepository)
        {
            this.logQueryRepository = logQueryRepository;
        }

        public async Task<IReadOnlyList<LogEntry>> Handle(ListAllEventsQueryRequest request, CancellationToken cancellationToken)
        {
            var events = await logQueryRepository.GetLogsAsync(request.Filter);

            ErrorBuilder.Create(404)
                    .WithTitle("Kayıt Bulunamadı")
                    .WithDescription("Log kayıtları bulunamadı. Lütfen Daha sonra tekrar deneyiniz.")
                    .ThrowIfNull(events);

            return events;
        }
    }
}
