using AutoMapper;
using KitchenDeliverySystem.Dto.Order;

namespace KitchenDeliverySystem.Infra.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<Domain.Entities.Order, OrderDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(dest => dest.Items));
        }
    }
}
