using ErrorOr;
using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Application.UseCases.User.UserLogin
{
    public interface IUserLoginUseCase : IUseCase
    {
        Task<ErrorOr<string>> ExecuteAsync(UserLoginDto inbound);
    }
}
