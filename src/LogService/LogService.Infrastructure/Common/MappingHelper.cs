using LogService.Application.DTOs;
using LogService.Domain.Entities;
using LogService.Infrastructure.Repositories.Seq.Models;

namespace LogService.Infrastructure.Common
{
    public static class MappingHelper
    {
        public static LogEntry MapToLogEntry(SeqEventItem item)
        {
            return new LogEntry
            {
                Id = item.Id,
                Timestamp = item.Timestamp,
                Level = item.Level,
                RenderedMessage = string.Join("", item.MessageTemplateTokens),
                Exception = item.Exception,
                Properties = item.Properties,
            };
        }

        public static DashboardEntry MapToDashboardEntry(SeqDashboardItem item)
        {
            return new DashboardEntry
            {
                Id = item.Id,
                Title = item.Title,
                IsProtected = item.IsProtected,
                Charts = item.Charts?.Select(c => new DashboardChart
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Queries = c.Queries?.Select(q => new DashboardChartQuery
                    {
                        Id = q.Id,
                        Measurements = q.Measurements?.Select(m => new DashboardMeasurement
                        {
                            Value = m.Value,
                            Label = m.Label
                        }).ToList() ?? new(),
                        Where = q.Where,
                        GroupBy = q.GroupBy ?? new(),
                        DisplayType = q.DisplayStyle?.Type ?? string.Empty
                    }).ToList() ?? new()
                }).ToList() ?? new()
            };
        }
    }
}
