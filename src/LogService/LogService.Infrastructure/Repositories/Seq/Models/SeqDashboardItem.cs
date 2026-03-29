using System.Text.Json.Serialization;

namespace LogService.Infrastructure.Repositories.Seq.Models
{
    public class SeqDashboardItem
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("Title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("IsProtected")]
        public bool IsProtected { get; set; }

        [JsonPropertyName("Charts")]
        public List<SeqChart>? Charts { get; set; }
    }

    public class SeqChart
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("Title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("Queries")]
        public List<SeqChartQuery>? Queries { get; set; }
    }

    public class SeqChartQuery
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("Measurements")]
        public List<SeqMeasurement>? Measurements { get; set; }

        [JsonPropertyName("Where")]
        public string? Where { get; set; }

        [JsonPropertyName("GroupBy")]
        public List<string>? GroupBy { get; set; }

        [JsonPropertyName("DisplayStyle")]
        public SeqDisplayStyle? DisplayStyle { get; set; }
    }

    public class SeqMeasurement
    {
        [JsonPropertyName("Value")]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("Label")]
        public string Label { get; set; } = string.Empty;
    }

    public class SeqDisplayStyle
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = string.Empty;
    }
}
