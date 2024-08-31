using ErrorOr;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderCreate
{
    public interface ICreateOrderUseCase
    { 
        Task<ErrorOr<OrderDto>> ExecuteAsync(CreateOderDto inbound);
    }
}
