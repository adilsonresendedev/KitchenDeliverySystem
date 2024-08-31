using AutoMapper;
using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Filters;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Dto.Pagination;

namespace KitchenDeliverySystem.Application.UseCases.Order.OrderSearch
{
    public class SearchOrderUseCase : ISearchOrderUseCase
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public SearchOrderUseCase(
            IMapper mapper, 
            IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<ErrorOr<PagedResultDto<OrderDto>>> ExecuteAsync(OrderFilterDto inbound)
        {
            var filter = _mapper.Map<OrderFilter>(inbound);

            var (total, items)= await _orderRepository.SearchAsync(filter);
            if (total == 0)
                return ErrorCatalog.OrderNotFound;

            return new PagedResultDto<OrderDto>
            {
                PageNumber = inbound.PageNumber,
                PageSize = inbound.PageSize,
                Total = total,
                Data = _mapper.Map<List<OrderDto>>(items)
            };
        }
    }
}
