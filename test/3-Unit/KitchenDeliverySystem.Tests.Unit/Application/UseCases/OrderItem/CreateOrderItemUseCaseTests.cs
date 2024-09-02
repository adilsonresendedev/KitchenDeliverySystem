using AutoMapper;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemCreate;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.Order;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.OrderItem
{
    public class CreateOrderItemUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly IMapper _mapper;

        public CreateOrderItemUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();

            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KitchenDeliverySystem.Domain.Entities.OrderItem, OrderItemDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderExists_ShouldCreateOrderItemAndReturnOrderItemDto()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");
            var createOrderItemDto = new CreateOrderItemDto
            {
                Name = "Test Item",
                Quantity = 2,
                Notes = "Test Notes"
            };

            var useCase = new CreateOrderItemUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockOrderItemRepository.Setup(r => r.AddAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await useCase.ExecuteAsync(orderId, createOrderItemDto);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<OrderItemDto>();
            result.Value.OrderId.Should().Be(orderId);
            result.Value.Name.Should().Be(createOrderItemDto.Name);
            result.Value.Quantity.Should().Be(createOrderItemDto.Quantity);
            result.Value.Notes.Should().Be(createOrderItemDto.Notes);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.AddAsync(It.Is<KitchenDeliverySystem.Domain.Entities.OrderItem>(oi =>
                oi.OrderId == orderId &&
                oi.Name == createOrderItemDto.Name &&
                oi.Quantity == createOrderItemDto.Quantity &&
                oi.Notes == createOrderItemDto.Notes)), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderNotFound_ShouldReturnOrderNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var createOrderItemDto = new CreateOrderItemDto
            {
                Name = "Test Item",
                Quantity = 2,
                Notes = "Test Notes"
            };

            var useCase = new CreateOrderItemUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync((KitchenDeliverySystem.Domain.Entities.Order)null);

            // Act
            var result = await useCase.ExecuteAsync(orderId, createOrderItemDto);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderNotFound);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            _mockOrderItemRepository.Verify(r => r.AddAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenAddSucceeds_ShouldCallAddAsyncAndCommitAsync()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");
            var createOrderItemDto = new CreateOrderItemDto
            {
                Name = "Test Item",
                Quantity = 2,
                Notes = "Test Notes"
            };

            var useCase = new CreateOrderItemUseCase(_mockUnitOfWork.Object, _mockOrderRepository.Object, _mockOrderItemRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mockOrderItemRepository.Setup(r => r.AddAsync(It.IsAny<KitchenDeliverySystem.Domain.Entities.OrderItem>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            await useCase.ExecuteAsync(orderId, createOrderItemDto);

            // Assert
            _mockOrderItemRepository.Verify(r => r.AddAsync(It.Is<KitchenDeliverySystem.Domain.Entities.OrderItem>(oi =>
                oi.OrderId == orderId &&
                oi.Name == createOrderItemDto.Name &&
                oi.Quantity == createOrderItemDto.Quantity &&
                oi.Notes == createOrderItemDto.Notes)), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }
    }
}
