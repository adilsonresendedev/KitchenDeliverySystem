using AutoMapper;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.OrderItem.Update;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.Order;
using Moq;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.OrderItem
{
    public class UpdateOrderItemUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly IMapper _mapper;

        public UpdateOrderItemUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KitchenDeliverySystem.Domain.Entities.OrderItem, OrderItemDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderAndItemExist_ShouldUpdateAndReturnOrderItemDto()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            var inbound = new UpdateOrderItemDto { Name = "Updated Item", Quantity = 5, Notes = "Updated Notes" };

            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");
            var existingOrderItem = new KitchenDeliverySystem.Domain.Entities.OrderItem(1, "Old Item", 3, "notes");
            existingOrder.Items.Add(existingOrderItem);

            var useCase = new UpdateOrderItemUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);
            _mockOrderItemRepository.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync(existingOrderItem);
            _mockOrderItemRepository.Setup(r => r.UpdateAsync(existingOrderItem))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(orderId, itemId, inbound);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<OrderItemDto>();
            result.Value.Name.Should().Be(inbound.Name);
            result.Value.Quantity.Should().Be(inbound.Quantity);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.GetByIdAsync(itemId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.UpdateAsync(existingOrderItem), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderNotFound_ShouldReturnOrderNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            var inbound = new UpdateOrderItemDto { Name = "Updated Item", Quantity = 5, Notes = "Updated Notes" };

            var useCase = new UpdateOrderItemUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.Order)null);

            // Act
            var result = await useCase.ExecuteAsync(orderId, itemId, inbound);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderNotFound);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.GetByIdAsync(itemId), Times.Never);
            _mockOrderItemRepository.Verify(r => r.UpdateAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderItemNotFound_ShouldReturnOrderItemNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            var inbound = new UpdateOrderItemDto { Name = "Updated Item", Quantity = 5, Notes = "Updated Notes" };

            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");

            var useCase = new UpdateOrderItemUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);
            _mockOrderItemRepository.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.OrderItem)null);

            // Act
            var result = await useCase.ExecuteAsync(orderId, itemId, inbound);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderItemNotFound);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.GetByIdAsync(itemId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.UpdateAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
