using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace KitchenDeliverySystem.Infra.Persistence
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Set<User>().Where(u => u.IsActive).ToListAsync();
        }
    }
}
