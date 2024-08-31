using ErrorOr;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.OrderItem.Update
{
    public interface IUpdateOrderItemUseCase
    {
        Task<ErrorOr<OrderItemDto>> ExecuteAsync(int orderId, int itemId, UpdateOrderItemDto inbound);
    }
}
