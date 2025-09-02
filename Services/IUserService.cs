using AuditTrailAPI.DTO;
using AuditTrailAPI.Model;

namespace AuditTrailAPI.Services
{
    public interface IUserService
    {
        Task<User> CreateAsync(CreateUserDto dto, string actor, string? correlationId);
        Task<bool> UpdateAsync(int id, UpdateUserDto dto, string actor, string? correlationId);
        Task<bool> DeleteAsync(int id, string actor, string? correlationId);
        Task<User?> GetByIdAsync(int id);
    }
}
