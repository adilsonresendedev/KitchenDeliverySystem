using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderDelete
{
    public class DeleteOrderUseCase : IDeleteOrderUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderUseCase(
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
        }

        public async Task<ErrorOr<Success>> ExecuteAsync(int inbound)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(inbound);
            if (existingOrder is null)
                return ErrorCatalog.OrderNotFound;

            if (existingOrder.Items is not null && existingOrder.Items.Count > 0)
                return ErrorCatalog.OrderCantDeleteHasItems;

            await _orderRepository.Delete(existingOrder);
            await _unitOfWork.CommitAsync();

            return new Success();
        }
    }
}
