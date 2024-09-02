using AutoMapper;
using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Infra.Mappers;

namespace KitchenDeliverySystem.Test.Unit.Infra.Mappers
{
    public class OrderProfileTests
    {
        private readonly IMapper _mapper;

        public OrderProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderProfile>();
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
        public void Should_Map_Order_To_OrderDto()
        {
            // Arrange
            var order = new Order("Sample Customer");

            var orderItem = new OrderItem(123, "Sample Item", 10.5m, "Sample notes");
            order.AddItem(orderItem);

            var orderDto = new OrderDto
            {
                CustomerName = "Sample Customer",
                OrderTime = new DateTime(2024, 8, 30),
                OrderStatus = OrderStatus.Pending,
                Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    OrderId = 123,
                    Name = "Sample Item",
                    Quantity = 10.5m,
                    Notes = "Sample notes"
                }
            }
            };

            // Act
            var result = _mapper.Map<OrderDto>(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.CustomerName, result.CustomerName);
            Assert.Equal(order.OrderTime, result.OrderTime);
            Assert.Equal(order.OrderStatus, result.OrderStatus);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);

            var resultItem = result.Items.First();
            var expectedItem = orderDto.Items.First();

            Assert.Equal(expectedItem.OrderId, resultItem.OrderId);
            Assert.Equal(expectedItem.Name, resultItem.Name);
            Assert.Equal(expectedItem.Quantity, resultItem.Quantity);
            Assert.Equal(expectedItem.Notes, resultItem.Notes);
        }

        [Fact]
        public void Should_Map_OrderDto_To_Order()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                CustomerName = "Sample Customer",
                OrderTime = new DateTime(2024, 8, 30),
                OrderStatus = OrderStatus.Pending,
                Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    OrderId = 123,
                    Name = "Sample Item",
                    Quantity = 10.5m,
                    Notes = "Sample notes"
                }
            }
            };

            // Act
            var order = new Order(orderDto.CustomerName);

            var result = _mapper.Map<Order>(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDto.CustomerName, result.CustomerName);
            Assert.Equal(orderDto.OrderTime, result.OrderTime);
            Assert.Equal(orderDto.OrderStatus, result.OrderStatus);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);

            var resultItem = result.Items.First();
            var expectedItem = orderDto.Items.First();

            Assert.Equal(expectedItem.OrderId, resultItem.OrderId);
            Assert.Equal(expectedItem.Name, resultItem.Name);
            Assert.Equal(expectedItem.Quantity, resultItem.Quantity);
            Assert.Equal(expectedItem.Notes, resultItem.Notes);
        }
    }
}
