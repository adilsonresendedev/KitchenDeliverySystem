using ErrorOr;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.Order.OrderDelete;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using Moq;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.Order
{
    public class DeleteOrderUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;

        public DeleteOrderUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderExists_ShouldDeleteOrderAndReturnSuccess()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");
            var useCase = new DeleteOrderUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockOrderRepository.Setup(r => r.DeleteAsync(existingOrder))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(orderId);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeOfType<Success>();

            _mockOrderRepository.Verify(r => r.DeleteAsync(existingOrder), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderNotFound_ShouldReturnOrderNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var useCase = new DeleteOrderUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.Order)null);

            // Act
            var result = await useCase.ExecuteAsync(orderId);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderNotFound);

            _mockOrderRepository.Verify(r => r.DeleteAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.Order>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderHasItems_ShouldReturnOrderCantDeleteHasItemsError()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");
            existingOrder.AddItem(new KitchenDeliverySystem.Domain.Entities.OrderItem(existingOrder.Id, "item name", 1, "item notes"));

            var useCase = new DeleteOrderUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            // Act
            var result = await useCase.ExecuteAsync(orderId);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderCantDeleteHasItems);

            _mockOrderRepository.Verify(r => r.DeleteAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.Order>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
