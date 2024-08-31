using AutoMapper;
using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Application.UseCases.OrderItem.Get
{
    public class GetOrderItemUseCase : IGetOrderItemUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public GetOrderItemUseCase(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<List<OrderItemDto>>> ExecuteAsync(int orderId)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(orderId);
            if (existingOrder is null)
                return ErrorCatalog.OrderNotFound;

            if (existingOrder.Items is null || !existingOrder.Items.Any())
                return ErrorCatalog.OrderItemsNotFound;

            var orderItemDtos = _mapper.Map<List<OrderItemDto>>(existingOrder.Items);

            return orderItemDtos;
        }
    }
}
