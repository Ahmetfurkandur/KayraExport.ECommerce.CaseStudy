namespace LogService.Domain.Entities
{
    public class LogEntry
    {
        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Level { get; set; }
        public string RenderedMessage { get; set; }
        public string? Exception { get; set; }
        public IReadOnlyList<Property>? Properties { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
