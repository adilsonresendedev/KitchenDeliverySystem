using ErrorOr;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemDelete;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.OrderItem
{
    public class DeleteOrderItemUseCaseTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public DeleteOrderItemUseCaseTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderAndOrderItemExist_ShouldDeleteOrderItemAndReturnSuccess()
        {
            // Arrange
            var orderId = 1;
            var orderItemId = 2;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");
            var existingOrderItem = new KitchenDeliverySystem.Domain.Entities.OrderItem(orderId, "Test Item", 1, "Test Notes");

            var useCase = new DeleteOrderItemUseCase(_mockOrderRepository.Object, _mockOrderItemRepository.Object, _mockUnitOfWork.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockOrderItemRepository.Setup(r => r.GetByIdAsync(orderItemId))
                .ReturnsAsync(existingOrderItem);

            _mockOrderItemRepository.Setup(r => r.DeleteAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(orderId, orderItemId);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeOfType<Success>();

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.GetByIdAsync(orderItemId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(existingOrderItem), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderNotFound_ShouldReturnOrderNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var orderItemId = 2;

            var useCase = new DeleteOrderItemUseCase(_mockOrderRepository.Object, _mockOrderItemRepository.Object, _mockUnitOfWork.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.Order)null);

            // Act
            var result = await useCase.ExecuteAsync(orderId, orderItemId);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderNotFound);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderItemNotFound_ShouldReturnOrderItemNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var orderItemId = 2;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");

            var useCase = new DeleteOrderItemUseCase(_mockOrderRepository.Object, _mockOrderItemRepository.Object, _mockUnitOfWork.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockOrderItemRepository.Setup(r => r.GetByIdAsync(orderItemId))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.OrderItem)null);

            // Act
            var result = await useCase.ExecuteAsync(orderId, orderItemId);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderItemNotFound);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.GetByIdAsync(orderItemId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenDeleteSucceeds_ShouldCallDeleteAsyncAndCommitAsync()
        {
            // Arrange
            var orderId = 1;
            var orderItemId = 2;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");
            var existingOrderItem = new KitchenDeliverySystem.Domain.Entities.OrderItem(orderId, "Test Item", 1, "Test Notes");

            var useCase = new DeleteOrderItemUseCase(_mockOrderRepository.Object, _mockOrderItemRepository.Object, _mockUnitOfWork.Object);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockOrderItemRepository.Setup(r => r.GetByIdAsync(orderItemId))
                .ReturnsAsync(existingOrderItem);

            _mockOrderItemRepository.Setup(r => r.DeleteAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            await useCase.ExecuteAsync(orderId, orderItemId);

            // Assert
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(existingOrderItem), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }
    }
}
