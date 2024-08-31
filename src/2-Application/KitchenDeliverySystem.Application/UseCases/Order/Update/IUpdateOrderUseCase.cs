using ErrorOr;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderUpdate
{
    public interface IUpdateOrderUseCase
    {
        Task<ErrorOr<OrderDto>> ExecuteAsync(int id, UpdateOrderDto inbound);
    } 
}
