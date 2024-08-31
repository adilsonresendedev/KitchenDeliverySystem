using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Infra.Context;

namespace KitchenDeliverySystem.Infra.Persistence
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
