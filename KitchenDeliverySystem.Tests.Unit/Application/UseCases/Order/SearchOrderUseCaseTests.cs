using AutoMapper;
using FluentAssertions;
using KitchenDeliverySystem.Application.UseCases.Order.OrderSearch;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Domain.Filters;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Dto.Pagination;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDeliverySystem.Test.Unit.Application.UseCases.Order
{
    public class SearchOrderUseCaseTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly IMapper _mapper;

        public SearchOrderUseCaseTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();

            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderFilterDto, OrderFilter>();
                cfg.CreateMap<KitchenDeliverySystem.Domain.Entities.Order, OrderDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_WhenOrdersFound_ShouldReturnPagedResultDto()
        {
            // Arrange
            var filterDto = new OrderFilterDto
            {
                CustomerName = "John",
                PageNumber = 1,
                PageSize = 10
            };

            var orders = new List<KitchenDeliverySystem.Domain.Entities.Order>
            {
                new KitchenDeliverySystem.Domain.Entities.Order("John Doe"),
                new KitchenDeliverySystem.Domain.Entities.Order("John Smith")
            };

            var useCase = new SearchOrderUseCase(_mapper, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.SearchAsync(It.IsAny<OrderFilter>()))
                .ReturnsAsync((20, orders));

            // Act
            var result = await useCase.ExecuteAsync(filterDto);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<PagedResultDto<OrderDto>>();
            result.Value.Total.Should().Be(20);
            result.Value.Data.Should().HaveCount(2);
            result.Value.PageNumber.Should().Be(1);
            result.Value.PageSize.Should().Be(10);

            _mockOrderRepository.Verify(r => r.SearchAsync(It.Is<OrderFilter>(f =>
                f.CustomerName == filterDto.CustomerName &&
                f.PageNumber == filterDto.PageNumber &&
                f.PageSize == filterDto.PageSize)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenNoOrdersFound_ShouldReturnOrderNotFoundError()
        {
            // Arrange
            var filterDto = new OrderFilterDto
            {
                CustomerName = "NonExistent",
                PageNumber = 1,
                PageSize = 10
            };

            var useCase = new SearchOrderUseCase(_mapper, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.SearchAsync(It.IsAny<OrderFilter>()))
                .ReturnsAsync((0, new List<KitchenDeliverySystem.Domain.Entities.Order>()));

            // Act
            var result = await useCase.ExecuteAsync(filterDto);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ErrorCatalog.OrderNotFound);

            _mockOrderRepository.Verify(r => r.SearchAsync(It.Is<OrderFilter>(f =>
                f.CustomerName == filterDto.CustomerName &&
                f.PageNumber == filterDto.PageNumber &&
                f.PageSize == filterDto.PageSize)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithComplexFilter_ShouldPassCorrectFilterToRepository()
        {
            // Arrange
            var filterDto = new OrderFilterDto
            {
                CustomerName = "John",
                OrderTimeStart = new DateTime(2023, 1, 1),
                OrderTimeEnd = new DateTime(2023, 12, 31),
                OrderStatus = OrderStatus.Completed,
                PageNumber = 2,
                PageSize = 15
            };

            var useCase = new SearchOrderUseCase(_mapper, _mockOrderRepository.Object);

            _mockOrderRepository.Setup(r => r.SearchAsync(It.IsAny<OrderFilter>()))
                .ReturnsAsync((30, new List<KitchenDeliverySystem.Domain.Entities.Order>()));

            // Act
            await useCase.ExecuteAsync(filterDto);

            // Assert
            _mockOrderRepository.Verify(r => r.SearchAsync(It.Is<OrderFilter>(f =>
                f.CustomerName == filterDto.CustomerName &&
                f.OrderTimeStart == filterDto.OrderTimeStart &&
                f.OrderTimeEnd == filterDto.OrderTimeEnd &&
                f.OrderStatus == filterDto.OrderStatus &&
                f.PageNumber == filterDto.PageNumber &&
                f.PageSize == filterDto.PageSize)), Times.Once);
        }
    }
}
