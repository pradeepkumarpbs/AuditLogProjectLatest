using AuditTrailAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditTrailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _audit;
        public AuditLogController(IAuditLogService audit) => _audit = audit;

        // GET /api/auditlogs?entityName=User&entityId=5&page=1&pageSize=50
        [HttpGet]
        public async Task<ActionResult> List(
            [FromQuery] string? entityName,
            [FromQuery] string? entityId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var (total, items) = await _audit.ListAsync(entityName, entityId, page, pageSize);
            return Ok(new { total, page, pageSize, items });
        }
    }
}
