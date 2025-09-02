using AuditTrailAPI.Model;

namespace AuditTrailAPI.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(string entityName, string entityId, AuditAction action,
                  string changesJson, string actor, string? correlationId);

        Task<(int total, List<AuditLog> items)> ListAsync(string? entityName, string? entityId,
                      int page, int pageSize);
    }
}
