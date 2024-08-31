using ErrorOr;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.OrderItem.Get
{
    public interface IGetOrderItemUseCase
    {
        Task<ErrorOr<List<OrderItemDto>>> ExecuteAsync(int inbound);
    }
}
