using ErrorOr;
using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Application.UseCases.User.UserInsert
{
    public interface ICreateUserUseCase : IUseCase
    {
        Task<ErrorOr<UserDto>> ExecuteAsync(CreateUserDto inbound);
    }
}
