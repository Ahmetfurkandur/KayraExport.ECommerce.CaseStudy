using LogService.Domain.Entities;
using MediatR;

namespace LogService.Application.Features.Statistics.Queries.GetStatisticByIdQuery
{
    public class GetStatisticByIdQueryRequest : IRequest<DashboardEntry>
    {
        public string StatisticId { get; set; }

        public GetStatisticByIdQueryRequest(string statisticId)
        {
            StatisticId = statisticId;
        }
    }
}
