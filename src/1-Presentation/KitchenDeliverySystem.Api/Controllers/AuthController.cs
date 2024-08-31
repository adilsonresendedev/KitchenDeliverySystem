using KitchenDeliverySystem.Api.Validation;
using KitchenDeliverySystem.Application.UseCases.User.UserInsert;
using KitchenDeliverySystem.Application.UseCases.User.UserLogin;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDeliverySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICreateUserUseCase _userCreateUseCase;
        private readonly IUserLoginUseCase _userLoginUseCase;

        public AuthController(ICreateUserUseCase userCreateUseCase, IUserLoginUseCase userLoginUseCase)
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
            if (result.IsError)
            {
                var error = result.Errors.FirstOrDefault();
                return UnprocessableEntity(error);
            }
            return Ok(result.Value);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var result = await _userLoginUseCase.ExecuteAsync(userLoginDto);

            if (result.IsError)
            {
                var error = result.Errors.FirstOrDefault();
                if (error == ErrorCatalog.UserNotFound)
                {
                    return NotFound(error.Description);
                }
                else if (error == ErrorCatalog.UserInvalidPassword)
                {
                    return BadRequest(error.Description);
                }
            }

            var token = result.Value;
            return Ok(new { Token = token });
        }
    }
}
