using AutoMapper;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.Order.OrderGet;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Dto.Order;
using Moq;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.Order
{
    public class GetOrderUseCaseTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly IMapper _mapper;

        public GetOrderUseCaseTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KitchenDeliverySystem.Domain.Entities.Order, OrderDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderExists_ShouldReturnOrderDto()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new KitchenDeliverySystem.Domain.Entities.Order("Test Customer");
            var useCase = new GetOrderUseCase(_mockOrderRepository.Object, _mapper);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(existingOrder);

            // Act
            var result = await useCase.ExecuteAsync(orderId);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<OrderDto>();
            result.Value.CustomerName.Should().Be(existingOrder.CustomerName);

            _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrderNotFound_ShouldReturnOrderNotFoundError()
        {
            // Arrange
            var orderId = 1;
            var useCase = new GetOrderUseCase(_mockOrderRepository.Object, _mapper);

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
