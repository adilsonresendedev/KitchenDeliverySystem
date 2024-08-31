using ErrorOr;

namespace KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemDelete
{
    public interface IOrdemItemDeleteUseCase
    {
        Task<ErrorOr<Success>> ExecuteAsync(int orderId, int inbound);
    }
}
