using AutoMapper;
using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderUpdate
{
    public class UpdateOrderUseCase : IUpdateOrderUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<ErrorOr<OrderDto>> ExecuteAsync(int id, UpdateOrderDto inbound)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder is null)
                return ErrorCatalog.OrderNotFound;

            existingOrder.Update(
                inbound.CustomerName,
                inbound.OrderStatus);

            await _orderRepository.Update(existingOrder);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<OrderDto>(existingOrder);
        }
    }
}
