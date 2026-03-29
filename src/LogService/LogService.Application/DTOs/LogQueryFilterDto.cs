namespace LogService.Application.DTOs
{
    public class LogQueryFilterDto
    {
        public string? Filter { get; set; }          // Seq filter expression: "@Level = 'Error'"
        public int Count { get; set; } = 30;         // Seq varsayılanı 30
        public string? AfterId { get; set; }         // Cursor-based pagination
        public DateTimeOffset? FromDateUtc { get; set; }
        public DateTimeOffset? ToDateUtc { get; set; }
        public bool Render { get; set; } = true;     // RenderedMessage doldurulsun mu
    }
}
