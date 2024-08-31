using KitchenDeliverySystem.Domain.Entities;

namespace KitchenDeliverySystem.Domain.Repositories
{
    public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
        Task<List<OrderItem>> GetByOrderIdAsync(int inbound);
    }
}
