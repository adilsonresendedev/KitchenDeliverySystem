using AutoMapper;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.Order.OrderUpdate;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.Order;
using Moq;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.Order
{
    public class UpdateOrderUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly IMapper _mapper;

        public UpdateOrderUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KitchenDeliverySystem.Domain.Entities.Order, OrderDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderExists_ShouldUpdateOrderAndReturnOrderDto()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Old Customer");
            var updateOrderDto = new UpdateOrderDto
            {
                CustomerName = "New Customer",
                OrderStatus = OrderStatus.Completed
            };

            var useCase = new UpdateOrderUseCase(_mockUnitOfWork.Object, _mapper, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockOrderRepository.Setup(r => r.UpdateAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.Order>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(orderId, updateOrderDto);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<OrderDto>();
            result.Value.CustomerName.Should().Be(updateOrderDto.CustomerName);
            result.Value.OrderStatus.Should().Be(updateOrderDto.OrderStatus);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderNotFound_ShouldReturnOrderNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var updateOrderDto = new UpdateOrderDto
            {
                CustomerName = "New Customer",
                OrderStatus = OrderStatus.Completed
            };

            var useCase = new UpdateOrderUseCase(_mockUnitOfWork.Object, _mapper, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.Order)null);

            // Act
            var result = await useCase.ExecuteAsync(orderId, updateOrderDto);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderNotFound);

            _mockOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.Order>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
