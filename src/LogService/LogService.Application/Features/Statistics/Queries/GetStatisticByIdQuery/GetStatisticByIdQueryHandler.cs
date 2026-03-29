using ErrorHandling;
using LogService.Application.Interfaces;
using LogService.Domain.Entities;
using MediatR;

namespace LogService.Application.Features.Statistics.Queries.GetStatisticByIdQuery
{
    public class GetStatisticByIdQueryHandler : IRequestHandler<GetStatisticByIdQueryRequest, DashboardEntry>
    {
        private readonly ILogQueryRepository logQueryRepository;

        public GetStatisticByIdQueryHandler(ILogQueryRepository logQueryRepository)
        {
            this.logQueryRepository = logQueryRepository;
        }

        public async Task<DashboardEntry> Handle(GetStatisticByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var dashboard = await logQueryRepository.GetDashboardByIdAsync(request.StatisticId);

            ErrorBuilder.Create(404)
                    .WithTitle("Kayıt Bulunamadı")
                    .WithDescription("İstatistik kaydı bulunamadı. Lütfen Daha sonra tekrar deneyiniz.")
                    .ThrowIfNull(dashboard);

            return dashboard;
        }
    }
}
