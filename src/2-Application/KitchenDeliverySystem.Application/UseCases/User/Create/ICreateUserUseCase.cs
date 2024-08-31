using ErrorOr;
using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Application.UseCases.User.UserInsert
{
    public interface ICreateUserUseCase
    {
        Task<ErrorOr<UserDto>> ExecuteAsync(CreateUserDto inbound);
    }
}
