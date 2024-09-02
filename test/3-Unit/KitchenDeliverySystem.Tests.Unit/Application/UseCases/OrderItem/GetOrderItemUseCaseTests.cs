using AutoMapper;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.OrderItem.Get;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Dto.Order;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.OrderItem
{
    public class GetOrderItemUseCaseTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly IMapper _mapper;

        public GetOrderItemUseCaseTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KitchenDeliverySystem.Domain.Entities.OrderItem, OrderItemDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderExistsWithItems_ShouldReturnOrderItemDtos()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");

            var item1 = new KitchenDeliverySystem.Domain.Entities.OrderItem(1, "Item 1", 2, "notes 1");
            var item2 = new KitchenDeliverySystem.Domain.Entities.OrderItem(1, "Item 2", 2, "notes 1");

            existingOrder.AddItem(item1);
            existingOrder.AddItem(item2);

            var useCase = new GetOrderItemUseCase(_mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            // Act
            var result = await useCase.ExecuteAsync(orderId);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<List<OrderItemDto>>();
            result.Value.Count.Should().Be(existingOrder.Items.Count);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderExistsWithoutItems_ShouldReturnOrderItemsNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");

            var useCase = new GetOrderItemUseCase(_mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            // Act
            var result = await useCase.ExecuteAsync(orderId);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderItemsNotFound);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderNotFound_ShouldReturnOrderNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var useCase = new GetOrderItemUseCase(_mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.Order)null);

            // Act
            var result = await useCase.ExecuteAsync(orderId);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderNotFound);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        }
    }
}
