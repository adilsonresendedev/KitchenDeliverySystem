using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Filters;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace KitchenDeliverySystem.Infra.Persistence
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {

        }

        public override async Task<Order> GetByIdAsync(int id)
        {
            var result = await _context.Set<Order>()
                .Include(x => x.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return result;
        }

        public async Task<(int total, List<Order> items)> SearchAsync(OrderFilter orderFilter)
        {
            var query = _context.Set<Order>()
                .AsQueryable();

            if (!string.IsNullOrEmpty(orderFilter.CustomerName))
            {
                query = query.Where(o => o.CustomerName.Contains(orderFilter.CustomerName));
            }

            if (orderFilter.OrderTimeStart.HasValue)
            {
                query = query.Where(o => o.OrderTime >= orderFilter.OrderTimeStart.Value);
            }

            if (orderFilter.OrderTimeEnd.HasValue)
            {
                query = query.Where(o => o.OrderTime <= orderFilter.OrderTimeEnd.Value);
            }

            if (orderFilter.OrderStatus.HasValue)
            {
                query = query.Where(o => o.OrderStatus == orderFilter.OrderStatus.Value);
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .Skip((orderFilter.PageNumber - 1) * orderFilter.PageSize)
                .Take(orderFilter.PageSize)
                .Include(o => o.Items)
                .AsNoTracking()
                .ToListAsync();

            return (totalRecords, items);
        }
    }
}
