using LogService.Domain.Entities;
using MediatR;

namespace LogService.Application.Features.Statistics.Queries.ListAllStatisticsQuery
{
    public class ListAllStatisticsQueryRequest : IRequest<IReadOnlyList<DashboardEntry>>
    {
    }
}
