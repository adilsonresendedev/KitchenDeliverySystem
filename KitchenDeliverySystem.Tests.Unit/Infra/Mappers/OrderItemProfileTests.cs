using AutoMapper;
using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Infra.Mappers;

namespace KitchenDeliverySystem.Test.Unit.Infra.Mappers
{
    public class OrderItemProfileTests
    {
        private readonly IMapper _mapper;

        public OrderItemProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderItemProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Should_Have_Valid_Configuration()
        {
            // Act & Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_OrderItem_To_OrderItemDto()
        {
            // Arrange
            var orderItem = new OrderItem(123, "Sample Item", 10.5m, "Sample notes");

            // Act
            var result = _mapper.Map<OrderItemDto>(orderItem);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderItem.Name, result.Name);
            Assert.Equal(orderItem.Quantity, result.Quantity);
            Assert.Equal(orderItem.Notes, result.Notes);
        }

        [Fact]
        public void Should_Map_OrderItemDto_To_OrderItem()
        {
            // Arrange
            var dto = new OrderItemDto
            {
                Id = 1,
                OrderId = 123,
                Name = "Sample Item",
                Quantity = 10.5m,
                Notes = "Sample notes"
            };

            // Act
            var orderItem = new OrderItem(dto.OrderId, dto.Name, dto.Quantity, dto.Notes);

            // Assert
            var result = _mapper.Map<OrderItemDto>(orderItem);

            Assert.NotNull(result);
            Assert.Equal(dto.OrderId, result.OrderId);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Quantity, result.Quantity);
            Assert.Equal(dto.Notes, result.Notes);
        }
    }
}
