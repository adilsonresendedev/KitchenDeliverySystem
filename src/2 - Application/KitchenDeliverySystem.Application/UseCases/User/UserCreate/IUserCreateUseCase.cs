using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Application.UseCases.User.UserInsert
{
    public interface IUserCreateUseCase
    {
        Task<UserDto> ExecuteAsync(CreateUserDto inbound);
    }
}
