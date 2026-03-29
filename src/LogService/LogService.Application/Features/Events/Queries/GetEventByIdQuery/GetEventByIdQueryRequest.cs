using LogService.Domain.Entities;
using MediatR;

namespace LogService.Application.Features.Events.Queries.GetEventByIdQuery
{
    public class GetEventByIdQueryRequest : IRequest<LogEntry>
    {
        public string EventId { get; set; }

        public GetEventByIdQueryRequest(string eventId)
        {
            EventId = eventId;
        }
    }
}
