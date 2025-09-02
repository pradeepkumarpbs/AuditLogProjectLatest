using AuditTrailAPI.Data;
using AuditTrailAPI.DTO;
using AuditTrailAPI.DTO.Utils;
using AuditTrailAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AuditTrailAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IAuditLogService _audit;

        public UserService(AppDbContext db, IAuditLogService audit)
        {
            _db = db;
            _audit = audit;
        }

        public async Task<User> CreateAsync(CreateUserDto dto, string actor, string? correlationId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // insert data to Audit log table for activty log
            var changes = Helper.BuildChangesJson<User>(null, user, "Id", "CreatedAt", "UpdatedAt");
            await _audit.LogAsync(nameof(User), user.Id.ToString(), AuditAction.Created, changes, actor, correlationId);

            await tx.CommitAsync();
            return user;
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto, string actor, string? correlationId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null) return false;

            var before = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            if (dto.FirstName is not null) user.FirstName = dto.FirstName;
            if (dto.LastName is not null) user.LastName = dto.LastName;
            if (dto.Email is not null) user.Email = dto.Email;
            user.UpdatedAt = DateTime.UtcNow;

            var changesJson = Helper.BuildChangesJson(before, user, "Id", "CreatedAt", "UpdatedAt");
            if (changesJson == "{}") return true;

            await _db.SaveChangesAsync();

            // insert data to Audit log table for activty log
            await _audit.LogAsync(nameof(User), user.Id.ToString(), AuditAction.Updated, changesJson, actor, correlationId);
            await tx.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string actor, string? correlationId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null) return false;

            var before = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            // insert data to Audit log table for activty log
            var changes = Helper.BuildChangesJson(before, null, "Id", "CreatedAt", "UpdatedAt");
            await _audit.LogAsync(nameof(User), id.ToString(), AuditAction.Deleted, changes, actor, correlationId);

            await tx.CommitAsync();
            return true;
        }

        public Task<User?> GetByIdAsync(int id)
            => _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}
