using AutoMapper;
using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemCreate
{
    public class CreateOrderItemUseCase : ICreateOrderItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepositoy;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public CreateOrderItemUseCase(
            IUnitOfWork unitOfWork, 
            IOrderRepository orderRepositoy, 
            IOrderItemRepository orderItemRepository, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _orderRepositoy = orderRepositoy;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<OrderItemDto>> ExecuteAsync(int orderId, CreateOrderItemDto inbound)
        {
           var existingOrder = await _orderRepositoy.GetByIdAsync(orderId);
            if (existingOrder is null)
                return ErrorCatalog.OrderNotFound;

            var orderItem = new Domain.Entities.OrderItem(
                orderId,
                inbound.Name,
                inbound.Quantity,
                inbound.Notes);

            await _orderItemRepository.AddAsync(orderItem);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<OrderItemDto>(orderItem);
        }
    }
}
