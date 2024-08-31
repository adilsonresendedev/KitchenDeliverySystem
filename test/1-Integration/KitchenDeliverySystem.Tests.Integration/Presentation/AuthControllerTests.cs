using Bogus;
using KitchenDeliverySystem.Api.Controllers;
using KitchenDeliverySystem.Application.UseCases.User.UserInsert;
using KitchenDeliverySystem.Application.UseCases.User.UserLogin;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Dto.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KitchenDeliverySystem.Test.Integration.Presentation
{
    public class AuthControllerTests
    {
        private readonly Mock<ICreateUserUseCase> _mockCreateUserUseCase;
        private readonly Mock<IUserLoginUseCase> _mockUserLoginUseCase;
        private readonly AuthController _controller;
        private readonly Faker<CreateUserDto> _createUserFaker;
        private readonly Faker<UserLoginDto> _userLoginFaker;

        public AuthControllerTests()
        {
            _mockCreateUserUseCase = new Mock<ICreateUserUseCase>();
            _mockUserLoginUseCase = new Mock<IUserLoginUseCase>();
            _controller = new AuthController(_mockCreateUserUseCase.Object, _mockUserLoginUseCase.Object);

            _createUserFaker = new Faker<CreateUserDto>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.Password, f => f.Internet.Password(8));

            _userLoginFaker = new Faker<UserLoginDto>()
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.Password, f => f.Internet.Password(8));
        }

        [Fact]
        public async Task Register_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var createUserDto = _createUserFaker.Generate();
            _mockCreateUserUseCase.Setup(x => x.ExecuteAsync(
                It.IsAny<CreateUserDto>()))
                .ReturnsAsync(new UserDto());

            // Act
            var result = await _controller.Register(createUserDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockCreateUserUseCase.Verify(x => x.ExecuteAsync(It.IsAny<CreateUserDto>()), Times.Once);
        }

        [Fact]
        public async Task Register_InvalidInput_ReturnsUnprocessableEntity()
        {
            // Arrange
            var invalidCreateUserDto = _createUserFaker.Clone()
                .RuleFor(u => u.FirstName, f => f.Random.String(51))
                .Generate();

            // Act
            var result = await _controller.Register(invalidCreateUserDto);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            _mockCreateUserUseCase.Verify(x => x.ExecuteAsync(It.IsAny<CreateUserDto>()), Times.Never);
        }

        [Fact]
        public async Task Register_UseCaseFailure_ReturnsBadRequest()
        {
            // Arrange
            var createUserDto = _createUserFaker.Generate();
            _mockCreateUserUseCase.Setup(x => x.ExecuteAsync(It.IsAny<CreateUserDto>()))
                .ReturnsAsync(ErrorCatalog.UserAlterdyExists);

            // Act
            var result = await _controller.Register(createUserDto);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            _mockCreateUserUseCase.Verify(x => x.ExecuteAsync(It.IsAny<CreateUserDto>()), Times.Once);
        }

        [Fact]
        public async Task Login_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userLoginDto = _userLoginFaker.Generate();
            _mockUserLoginUseCase.Setup(x => x.ExecuteAsync(It.IsAny<UserLoginDto>()))
                .ReturnsAsync(string.Empty);

            // Act
            var result = await _controller.Login(userLoginDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockUserLoginUseCase.Verify(x => x.ExecuteAsync(It.IsAny<UserLoginDto>()), Times.Once);
        }

        [Fact]
        public async Task Login_UseCaseFailure_ReturnsBadRequest()
        {
            // Arrange
            var userLoginDto = _userLoginFaker.Generate();
            _mockUserLoginUseCase.Setup(x => x.ExecuteAsync(It.IsAny<UserLoginDto>()))
                .ReturnsAsync(ErrorCatalog.UserInvalidPassword);

            // Act
            var result = await _controller.Login(userLoginDto);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            _mockUserLoginUseCase.Verify(x => x.ExecuteAsync(It.IsAny<UserLoginDto>()), Times.Once);
        }
    }
}
