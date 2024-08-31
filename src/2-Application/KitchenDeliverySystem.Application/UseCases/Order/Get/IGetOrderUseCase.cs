using ErrorOr;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderGet
{
    public interface IGetOrderUseCase
    {
        Task<ErrorOr<OrderDto>> ExecuteAsync(int inbound);
    }
}
