using AutoMapper;
using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.OrderItem.Update
{
    public class UpdateOrderItemUseCase : IUpdateOrderItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public UpdateOrderItemUseCase(
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<OrderItemDto>> ExecuteAsync(int orderId, int itemId, UpdateOrderItemDto inbound)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(orderId);
            if (existingOrder is null)
                return ErrorCatalog.OrderNotFound;

            var existingOrderItem = await _orderItemRepository.GetByIdAsync(itemId);
            if (existingOrderItem is null)
                return ErrorCatalog.OrderItemNotFound;

            existingOrderItem.Update(
                inbound.Name,
                inbound.Quantity,
                inbound.Notes);

            await _orderItemRepository.UpdateAsync(existingOrderItem);
            await _unitOfWork.CommitAsync();

            var result = _mapper.Map<OrderItemDto>(existingOrderItem);
            return result;
        }
    }
}
