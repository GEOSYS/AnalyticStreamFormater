namespace QuickStarted
{
    public class AnalyticsDto
    {
        public EntityDto Entity { get; set; }
        public AnalyticsSchemaDto Schema { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> Values { get; set; }
    }

    public class EntityDto
    {
        public string Id { get; set; }
        public string? CustomerId { get; set; }
        public string Type { get; set; }
    }

    public class AnalyticsSchemaDto
    {
        public string Id { get; set; }
        public SchemaVersionDto Version { get; set; }
    }

    public class SchemaVersionDto
    {
        public int Major { get; set; }
        public int Minor { get; set; }
    }
}
