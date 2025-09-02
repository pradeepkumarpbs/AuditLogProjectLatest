namespace AuditTrailAPI.Model
{
    public class AuditLog
    {
        public long Id { get; set; }
        public string EntityName { get; set; } = "";
        public string EntityId { get; set; } = "";
        public AuditAction Action { get; set; }
        public string ChangesJson { get; set; } = "{}";

        public string? UserId { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.Now;
        public string? CorrelationId { get; set; } 
    }

    public enum AuditAction
    {
        Created = 1,
        Updated = 2,
        Deleted = 3
    }
}
