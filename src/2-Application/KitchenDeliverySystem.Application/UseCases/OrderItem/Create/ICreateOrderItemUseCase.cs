using ErrorOr;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemCreate
{
    public interface ICreateOrderItemUseCase
    {
        Task<ErrorOr<OrderItemDto>> ExecuteAsync(int orderId, CreateOrderItemDto inbound);
    }
}
