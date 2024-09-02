using AutoMapper;
using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Domain.Filters;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Infra.Mappers;

namespace KitchenDeliverySystem.Test.Unit.Infra.Mappers
{
    public class OrderFilterProfileTests
    {
        private readonly IMapper _mapper;

        public OrderFilterProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderFilterProfile>();
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
        public void Should_Map_OrderFilterDto_To_OrderFilter()
        {
            // Arrange
            var dto = new OrderFilterDto
            {
                CustomerName = "John Doe",
                OrderTimeStart = new DateTime(2024, 1, 1),
                OrderTimeEnd = new DateTime(2024, 12, 31),
                OrderStatus = OrderStatus.Pending,
                PageNumber = 1,
                PageSize = 20
            };

            // Act
            var result = _mapper.Map<OrderFilter>(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.CustomerName, result.CustomerName);
            Assert.Equal(dto.OrderTimeStart, result.OrderTimeStart);
            Assert.Equal(dto.OrderTimeEnd, result.OrderTimeEnd);
            Assert.Equal(dto.OrderStatus, result.OrderStatus);
            Assert.Equal(dto.PageNumber, result.PageNumber);
            Assert.Equal(dto.PageSize, result.PageSize);
        }

        [Fact]
        public void Should_Map_OrderFilter_To_OrderFilterDto()
        {
            // Arrange
            var orderFilter = new OrderFilter
            {
                CustomerName = "Jane Doe",
                OrderTimeStart = new DateTime(2024, 2, 1),
                OrderTimeEnd = new DateTime(2024, 11, 30),
                OrderStatus = OrderStatus.Completed,
                PageNumber = 2,
                PageSize = 50
            };

            // Act
            var result = _mapper.Map<OrderFilterDto>(orderFilter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderFilter.CustomerName, result.CustomerName);
            Assert.Equal(orderFilter.OrderTimeStart, result.OrderTimeStart);
            Assert.Equal(orderFilter.OrderTimeEnd, result.OrderTimeEnd);
            Assert.Equal(orderFilter.OrderStatus, result.OrderStatus);
            Assert.Equal(orderFilter.PageNumber, result.PageNumber);
            Assert.Equal(orderFilter.PageSize, result.PageSize);
        }
    }
}
