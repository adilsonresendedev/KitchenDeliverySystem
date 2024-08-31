using AutoMapper;
using KitchenDeliverySystem.Domain.Filters;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Infra.Mappers
{
    public class OrderFilterProfile : Profile
    {
        public OrderFilterProfile()
        {
            CreateMap<OrderFilterDto, OrderFilter>()
            .ReverseMap();
        }
    }
}
