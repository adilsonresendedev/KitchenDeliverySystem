using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.CrossCutting.Utility;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.User;
using Microsoft.Extensions.Configuration;

namespace KitchenDeliverySystem.Application.UseCases.User.UserLogin
{
    public class UserLoginUseCase : IUserLoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserLoginUseCase(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<string>> ExecuteAsync(UserLoginDto inbound)
        {
            var user = await _userRepository.GetByUsernameAsync(inbound.UserName);
            if (user is null)
                return ErrorCatalog.UserNotFound;
            else if (PasswordUtility.CheckHash(inbound.Password, user.PasswordHash, user.PasswordSalt))
                return ErrorCatalog.UserInvalidPassword;

            var token = PasswordUtility.CreateToken(user, _configuration.GetSection("AppSettings:TokenKey").Value);

            return token;
        } 
    }
}
