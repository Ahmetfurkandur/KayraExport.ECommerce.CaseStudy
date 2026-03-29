using LogService.Domain.Entities;

namespace LogService.Application.DTOs
{

    public class SeqEventItem
    {
        public DateTime Timestamp { get; set; }
        public IReadOnlyList<Property> Properties { get; set; }
        public IReadOnlyList<MessageTemplateToken> MessageTemplateTokens { get; set; }
        public string EventType { get; set; }
        public string Level { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string SpanKind { get; set; }
        public string Id { get; set; }
        public string? Exception { get; set; }
        public Links Links { get; set; }
    }

    public class Links
    {
        public string Self { get; set; }
        public string Group { get; set; }
    }

    public class MessageTemplateToken
    {
        public string? Text { get; set; }
        public string? PropertyName { get; set; }
        public string? RawText { get; set; }
        public string? FormattedValue { get; set; }
    }

}
