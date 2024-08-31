using ErrorOr;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderDelete
{
    public interface IDeleteOrderUseCase
    {
        Task<ErrorOr<Success>> ExecuteAsync(int inbound);
    }
}
