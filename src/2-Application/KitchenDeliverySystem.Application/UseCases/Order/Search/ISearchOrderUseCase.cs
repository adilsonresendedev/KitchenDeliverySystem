using ErrorOr;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Dto.Pagination;
namespace KitchenDeliverySystem.Application.UseCases.Order.OrderSearch
{
    public interface ISearchOrderUseCase
    {
        Task<ErrorOr<PagedResultDto<OrderDto>>> ExecuteAsync(OrderFilterDto inbound);
    }
}
