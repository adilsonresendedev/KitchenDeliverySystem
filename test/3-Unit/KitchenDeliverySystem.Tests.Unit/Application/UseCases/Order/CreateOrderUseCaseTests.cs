using AutoMapper;
using Bogus;
using ErrorOr;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.Order.OrderCreate;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.Order;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.Order
{
    public class CreateOrderUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly IMapper _mapper;
        private readonly Faker<CreateOrderDto> _createOrderDtoFaker;

        public CreateOrderUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KitchenDeliverySystem.Domain.Entities.Order, OrderDto>();
            });
            _mapper = config.CreateMapper();

            _createOrderDtoFaker = new Faker<CreateOrderDto>()
                .RuleFor(o => o.CustomerName, f => f.Name.FullName());
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCreateOrderAndReturnOrderDto()
        {
            // Arrange
            var createOrderDto = _createOrderDtoFaker.Generate();
            var useCase = new CreateOrderUseCase(_mapper, _mockUnitOfWork.Object, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.Order>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(createOrderDto);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.CustomerName.Should().Be(createOrderDto.CustomerName);

            _mockOrderRepository.Verify(r => r.AddAsync(It.Is<KitchenDeliverySystem.Domain.Entities.Order>(o => o.CustomerName == createOrderDto.CustomerName)), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldNotSuppressExceptionWhenRepositoryThrowsException()
        {
            // Arrange
            var createOrderDto = _createOrderDtoFaker.Generate();
            var useCase = new CreateOrderUseCase(_mapper, _mockUnitOfWork.Object, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.Order>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(createOrderDto));

            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
