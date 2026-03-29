using LogService.Application.DTOs;
using LogService.Domain.Entities;

namespace LogService.Application.Interfaces
{
    /// <summary>
    /// S
    /// </summary>
    public interface ILogQueryRepository
    {
        Task<IReadOnlyList<LogEntry>> GetLogsAsync(LogQueryFilterDto filter, CancellationToken cancellationToken = default);
        Task<LogEntry?> GetLogByIdAsync(string eventId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<DashboardEntry>> GetDashboardsAsync(
    CancellationToken cancellationToken = default);
        Task<DashboardEntry> GetDashboardByIdAsync(string dashboardId,
    CancellationToken cancellationToken = default);
    }
}
