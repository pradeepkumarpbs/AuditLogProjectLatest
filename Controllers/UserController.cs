using AuditTrailAPI.DTO;
using AuditTrailAPI.Model;
using AuditTrailAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditTrailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _users;
        public UserController(IUserService users) => _users = users;

        private string Actor() => User?.Identity?.Name ?? Request.Headers["X-User-Id"].FirstOrDefault() ?? "system";
        private string? CorrelationId() => Request.Headers["X-Correlation-Id"].FirstOrDefault();

        [HttpPost]
        public async Task<ActionResult<User>> Create(CreateUserDto dto)
        {
            var created = await _users.CreateAsync(dto, Actor(), CorrelationId());
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var found = await _users.GetByIdAsync(id);
            return found is null ? NotFound() : Ok(found);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var ok = await _users.UpdateAsync(id, dto, Actor(), CorrelationId());
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _users.DeleteAsync(id, Actor(), CorrelationId());
            return ok ? NoContent() : NotFound();
        }
    }
}
