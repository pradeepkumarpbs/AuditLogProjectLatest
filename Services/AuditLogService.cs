using AuditTrailAPI.Data;
using AuditTrailAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AuditTrailAPI.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _db;
        public AuditLogService(AppDbContext db) => _db = db;

        public async Task LogAsync(string entityName, string entityId, AuditAction action,
                                   string changesJson, string actor, string? correlationId)
        {
            _db.AuditLogs.Add(new AuditLog
            {
                EntityName = entityName,
                EntityId = entityId,
                Action = action,
                ChangesJson = changesJson,
                UserId = actor,
                OccurredAt = DateTime.UtcNow,
                CorrelationId = correlationId
            });
            await _db.SaveChangesAsync();
        }

        public async Task<(int total, List<AuditLog> items)> ListAsync(
            string? entityName, string? entityId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 200);

            var q = _db.AuditLogs.AsNoTracking().OrderByDescending(x => x.OccurredAt).AsQueryable();
            if (!string.IsNullOrWhiteSpace(entityName)) q = q.Where(x => x.EntityName == entityName);
            if (!string.IsNullOrWhiteSpace(entityId)) q = q.Where(x => x.EntityId == entityId);

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (total, items);
        }
    }
}
