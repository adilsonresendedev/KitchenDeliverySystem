using AutoMapper;
using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Infra.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Domain.Entities.User, UserDto>()
                .ReverseMap();
        }
    }
}
