using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace KitchenDeliverySystem.Infra.Persistence
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<OrderItem>> GetByOrderIdAsync(int inbound)
        {
            var result = await _context.Set<OrderItem>()
                .Where(x => x.OrderId == inbound)
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}
