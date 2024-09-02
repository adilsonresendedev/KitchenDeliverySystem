using AutoMapper;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.User.UserInsert;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.User;
using Moq;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.User
{
    public class CreateUserUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;

        public CreateUserUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KitchenDeliverySystem.Domain.Entities.User, UserDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserDoesNotExist_ShouldCreateAndReturnUserDto()
        {
            // Arrange
            var inbound = new CreateUserDto
            {
                UserName = "newuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password123"
            };

            var passwordHashDto = new PasswordHashDto
            {
                PasswordHash = new byte[1],
                PasswordSalt = new byte[1]
            };

            var useCase = new CreateUserUseCase(_mockUnitOfWork.Object, _mockUserRepository.Object, _mapper);

            _mockUserRepository.Setup(r => r.GetByUsernameAsync(inbound.UserName))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.User)null);

            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.User>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(inbound);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<UserDto>();
            result.Value.Username.Should().Be(inbound.UserName);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserAlreadyExists_ShouldReturnUserAlreadyExistsError()
        {
            // Arrange
            var inbound = new CreateUserDto
            {
                UserName = "existinguser",
                FirstName = "Jane",
                LastName = "Doe",
                Password = "password123"
            };

            var existingUser = new KitchenDeliverySystem.Domain.Entities.User(
                true,
                "Jane",
                "Doe",
                "existinguser",
                new byte[1],
                new byte[1]);

            var useCase = new CreateUserUseCase(_mockUnitOfWork.Object, _mockUserRepository.Object, _mapper);

            _mockUserRepository.Setup(r => r.GetByUsernameAsync(inbound.UserName))
                .ReturnsAsync(existingUser);

            // Act
            var result = await useCase.ExecuteAsync(inbound);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.UserAlreadyExists);

            _mockUserRepository.Verify(r => r.GetByUsernameAsync(inbound.UserName), Times.Once);
            _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.User>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
