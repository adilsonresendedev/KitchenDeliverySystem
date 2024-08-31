using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.CrossCutting.Options;
using KitchenDeliverySystem.CrossCutting.Utility;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Dto.User;
using Microsoft.Extensions.Options;

namespace KitchenDeliverySystem.Application.UseCases.User.UserLogin
{
    public class UserLoginUseCase : IUserLoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public UserLoginUseCase(
        IUserRepository userRepository,
            IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<string>> ExecuteAsync(UserLoginDto inbound)
        {
            var user = await _userRepository.GetByUsernameAsync(inbound.UserName);
            if (user is null)
                return ErrorCatalog.UserNotFound;
            else if (!PasswordUtility.CheckHash(inbound.Password, user.PasswordHash, user.PasswordSalt))
                return ErrorCatalog.UserInvalidPassword;

            var token = PasswordUtility.CreateToken(user, _appSettings.TokenKey);

            return token;
        } 
    }
}
