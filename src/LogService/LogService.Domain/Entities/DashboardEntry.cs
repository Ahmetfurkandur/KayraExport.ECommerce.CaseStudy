namespace LogService.Domain.Entities
{
    public class DashboardEntry
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsProtected { get; set; }
        public List<DashboardChart> Charts { get; set; } = new();
    }

    public class DashboardChart
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<DashboardChartQuery> Queries { get; set; } = new();
    }

    public class DashboardChartQuery
    {
        public string Id { get; set; } = string.Empty;
        public List<DashboardMeasurement> Measurements { get; set; } = new();
        public string? Where { get; set; }
        public List<string> GroupBy { get; set; } = new();
        public string DisplayType { get; set; } = string.Empty; // Line, Pie, Bar, Value
    }

    public class DashboardMeasurement
    {
        public string Value { get; set; } = string.Empty;  // "count(*)", "count(distinct(@EventType))"
        public string Label { get; set; } = string.Empty;
    }
}
