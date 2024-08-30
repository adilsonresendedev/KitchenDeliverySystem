using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Application.UseCases.User.UserLogin
{
    public interface IUserLoginUseCase
    {
        Task<string> ExecuteAsync(UserLoginDto inbound);
    }
}
