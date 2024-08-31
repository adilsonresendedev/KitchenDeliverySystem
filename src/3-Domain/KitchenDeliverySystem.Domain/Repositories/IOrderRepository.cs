using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Filters;

namespace KitchenDeliverySystem.Domain.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<(int total, List<Order> items)> SearchAsync(OrderFilter orderFilter);
    }
}
