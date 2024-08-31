using AutoMapper;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Infra.Mappers
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<Domain.Entities.OrderItem, OrderItemDto>()
                .ReverseMap();
        }
    }
}
