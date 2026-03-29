using LogService.Application.DTOs;
using LogService.Domain.Entities;
using MediatR;

namespace LogService.Application.Features.Events.Queries.ListAllEventsQuery
{
    public class ListAllEventsQueryRequest : IRequest<IReadOnlyList<LogEntry>>
    {
        public LogQueryFilterDto Filter { get; set; }
    }
}
