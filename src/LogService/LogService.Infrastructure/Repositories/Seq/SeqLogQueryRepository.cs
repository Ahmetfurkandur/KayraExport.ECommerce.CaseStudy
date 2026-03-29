using LogService.Application.DTOs;
using LogService.Application.Interfaces;
using LogService.Domain.Entities;
using LogService.Infrastructure.Common;
using LogService.Infrastructure.Repositories.Seq.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace LogService.Infrastructure.Repositories.Seq
{
    public class SeqLogQueryRepository : ILogQueryRepository
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;

        public SeqLogQueryRepository(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
        }

        public async Task<LogEntry?> GetLogByIdAsync(string eventId, CancellationToken cancellationToken = default)
        {
            var url = $"/api/events/{Uri.EscapeDataString(eventId)}?render=true";

            var response = await client.GetAsync(url, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<SeqEventItem>(cancellationToken: cancellationToken);

            return json is null ? null : MappingHelper.MapToLogEntry(json);

        }

        public async Task<IReadOnlyList<LogEntry>> GetLogsAsync(LogQueryFilterDto? filter, CancellationToken cancellationToken = default)
        {
            var queryParams = new List<string>();

            //url'ye filtre parametreleri ekleme

            if (filter?.Count is not null)
                queryParams.Add($"count={filter?.Count}");

            if (!string.IsNullOrWhiteSpace(filter?.Filter))
                queryParams.Add($"filter={Uri.EscapeDataString(filter.Filter)}");

            if (filter?.Render == true)
                queryParams.Add($"render={Uri.EscapeDataString(filter!.Render.ToString().ToLower())}");

            if (!string.IsNullOrWhiteSpace(filter?.AfterId))
                queryParams.Add($"afterId={Uri.EscapeDataString(filter.AfterId)}");

            if (filter?.FromDateUtc.HasValue == true)
                queryParams.Add($"fromDateUtc={filter?.FromDateUtc.Value.UtcDateTime:O}");

            if (filter?.ToDateUtc.HasValue == true)
                queryParams.Add($"toDateUtc={filter?.ToDateUtc.Value.UtcDateTime:O}");

            var url = queryParams.Count > 0 ? $"api/events?{string.Join("&", queryParams)}" : "api/events";

            var response = await client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<List<SeqEventItem>>(cancellationToken: cancellationToken);

            return json?.Select(MappingHelper.MapToLogEntry).ToList().AsReadOnly()
                   ?? new List<LogEntry>().AsReadOnly();

        }

        public async Task<IReadOnlyList<DashboardEntry>> GetDashboardsAsync(
    CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync("/api/dashboards?shared=true", cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<List<SeqDashboardItem>>(
                cancellationToken: cancellationToken);

            return json?.Select(MappingHelper.MapToDashboardEntry).ToList().AsReadOnly()
                   ?? new List<DashboardEntry>().AsReadOnly();
        }

        public async Task<DashboardEntry> GetDashboardByIdAsync(string dashboardId, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync($"/api/dashboards/{dashboardId}", cancellationToken);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            var json = await response.Content.ReadFromJsonAsync<SeqDashboardItem>(
                cancellationToken: cancellationToken);

            return json is null ? null : MappingHelper.MapToDashboardEntry(json);
        }
    }
}
