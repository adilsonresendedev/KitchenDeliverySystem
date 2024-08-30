using KitchenDeliverySystem.Domain.Entities;

namespace KitchenDeliverySystem.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetActiveUsersAsync();
    }
}
