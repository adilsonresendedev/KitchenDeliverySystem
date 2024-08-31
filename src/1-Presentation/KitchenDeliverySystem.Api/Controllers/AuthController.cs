using KitchenDeliverySystem.Api.Controllers.Base;
using KitchenDeliverySystem.Api.Validation;
using KitchenDeliverySystem.Application.UseCases.User.UserInsert;
using KitchenDeliverySystem.Application.UseCases.User.UserLogin;
using KitchenDeliverySystem.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDeliverySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly ICreateUserUseCase _userCreateUseCase;
        private readonly IUserLoginUseCase _userLoginUseCase;

        public AuthController(
            ICreateUserUseCase userCreateUseCase, 
            IUserLoginUseCase userLoginUseCase)
        {
            _userCreateUseCase = userCreateUseCase;
            _userLoginUseCase = userLoginUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            var userValidator = new CreateUserDtoValidator();
            var validationResult = userValidator.Validate(createUserDto);

            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            var result = await _userCreateUseCase.ExecuteAsync(createUserDto);
            return HandleErrorOrResult(result);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var result = await _userLoginUseCase.ExecuteAsync(userLoginDto);
            return HandleErrorOrResult(result);
        }
    }
}
