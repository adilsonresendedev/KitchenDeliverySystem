using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;

namespace KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemDelete
{
    public class DeleteOrderItemUseCase : IOrdemItemDeleteUseCase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderItemUseCase(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> ExecuteAsync(int orderId, int orderItemId)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(orderId);
            if (existingOrder is null)
                return ErrorCatalog.OrderNotFound;

            var orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
            if (orderItem is null)
                return ErrorCatalog.OrderItemNotFound;

            await _orderItemRepository.DeleteAsync(orderItem);

            await _unitOfWork.CommitAsync();

            return new Success();
        }
    }
}
