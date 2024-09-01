using AutoMapper;
using ErrorOr;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderCreate
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;

        public CreateOrderUseCase(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<OrderDto>> ExecuteAsync(CreateOrderDto inbound)
        {
            var order = new Domain.Entities.Order
            (
                inbound.CustomerName
            );

            await _orderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<OrderDto>(order);
        }
    }
}
