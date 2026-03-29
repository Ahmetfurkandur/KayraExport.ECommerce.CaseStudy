using ErrorHandling;
using LogService.Application.Interfaces;
using LogService.Domain.Entities;
using MediatR;

namespace LogService.Application.Features.Statistics.Queries.ListAllStatisticsQuery
{
    public class ListAllStatisticsQueryHandler : IRequestHandler<ListAllStatisticsQueryRequest, IReadOnlyList<DashboardEntry>>
    {
        private readonly ILogQueryRepository logQueryRepository;

        public ListAllStatisticsQueryHandler(ILogQueryRepository logQueryRepository)
        {
            this.logQueryRepository = logQueryRepository;
        }
        public async Task<IReadOnlyList<DashboardEntry>> Handle(ListAllStatisticsQueryRequest request, CancellationToken cancellationToken)
        {
            var dashboards = await logQueryRepository.GetDashboardsAsync();

            ErrorBuilder.Create(404)
                    .WithTitle("Kayıt Bulunamadı")
                    .WithDescription("İstatistik kayıtları bulunamadı. Lütfen Daha sonra tekrar deneyiniz.")
                    .ThrowIfNull(dashboards);

            return dashboards;
        }
    }
}
