using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Infra.Context;

namespace KitchenDeliverySystem.Infra.Persistence
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
