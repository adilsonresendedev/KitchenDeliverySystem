using AutoMapper;
using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderGet
{
    public class GetOrderUseCase : IGetOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderUseCase(
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<OrderDto>> ExecuteAsync(int inbound)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(inbound);
            if (existingOrder is null)
                return ErrorCatalog.OrderNotFound;

            return _mapper.Map<OrderDto>(existingOrder);
        }
    }
}
