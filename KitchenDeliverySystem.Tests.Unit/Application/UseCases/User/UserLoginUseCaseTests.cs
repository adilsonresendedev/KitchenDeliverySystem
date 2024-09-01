using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.User.UserLogin;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.CrossCutting.Options;
using KitchenDeliverySystem.CrossCutting.Utility;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Dto.User;
using Microsoft.Extensions.Options;
using Moq;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.User
{
    public class UserLoginUseCaseTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly AppSettings _appSettings;
        private readonly string _tokenKey = "9d3063e0-5acc-404c-ac5c-3b8eb5ff61dd";

        public UserLoginUseCaseTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _appSettings = new AppSettings { TokenKey = _tokenKey };
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserExistsAndPasswordIsValid_ShouldReturnToken()
        {
            // Arrange
            var inbound = new UserLoginDto
            {
                UserName = "existinguser",
                Password = "validpassword"
            };

            var passwordHashDto = PasswordUtility.CreatePasswordHash(inbound.Password);

            var existingUser = new KitchenDeliverySystem.Domain.Entities.User(
                true,
                "John",
                "Doe",
                inbound.UserName,
                passwordHashDto.PasswordHash,
                passwordHashDto.PasswordSalt);

            var useCase = new UserLoginUseCase(_mockUserRepository.Object, Options.Create(_appSettings));

            _mockUserRepository.Setup(r => r.GetByUsernameAsync(inbound.UserName))
                .ReturnsAsync(existingUser);

            // Act
            var result = await useCase.ExecuteAsync(inbound);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<string>();
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserDoesNotExist_ShouldReturnUserNotFoundError()
        {
            // Arrange
            var inbound = new UserLoginDto
            {
                UserName = "nonexistentuser",
                Password = "somepassword"
            };

            var useCase = new UserLoginUseCase(_mockUserRepository.Object, Options.Create(_appSettings));

            _mockUserRepository.Setup(r => r.GetByUsernameAsync(inbound.UserName))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.User)null);

            // Act
            var result = await useCase.ExecuteAsync(inbound);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.UserNotFound);

            _mockUserRepository.Verify(r => r.GetByUsernameAsync(inbound.UserName), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenPasswordIsInvalid_ShouldReturnInvalidPasswordError()
        {
            // Arrange
            var inbound = new UserLoginDto
            {
                UserName = "existinguser",
                Password = "invalidpassword"
            };

            var passwordHashDto = PasswordUtility.CreatePasswordHash("correctpassword");

            var existingUser = new KitchenDeliverySystem.Domain.Entities.User(
                true,
                "John",
                "Doe",
                inbound.UserName,
                passwordHashDto.PasswordHash,
                passwordHashDto.PasswordSalt);

            var useCase = new UserLoginUseCase(_mockUserRepository.Object, Options.Create(_appSettings));

            _mockUserRepository.Setup(r => r.GetByUsernameAsync(inbound.UserName))
                .ReturnsAsync(existingUser);

            // Act
            var result = await useCase.ExecuteAsync(inbound);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.UserInvalidPassword);

            _mockUserRepository.Verify(r => r.GetByUsernameAsync(inbound.UserName), Times.Once);
        }
    }
}
