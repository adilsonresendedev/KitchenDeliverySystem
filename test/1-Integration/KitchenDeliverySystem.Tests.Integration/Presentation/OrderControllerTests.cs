using Bogus;
using ErrorOr;
using KitchenDeliverySystem.Api.Controllers;
using KitchenDeliverySystem.Application.UseCases.Order.OrderCreate;
using KitchenDeliverySystem.Application.UseCases.Order.OrderDelete;
using KitchenDeliverySystem.Application.UseCases.Order.OrderGet;
using KitchenDeliverySystem.Application.UseCases.Order.OrderSearch;
using KitchenDeliverySystem.Application.UseCases.Order.OrderUpdate;
using KitchenDeliverySystem.Application.UseCases.OrderItem.Get;
using KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemCreate;
using KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemDelete;
using KitchenDeliverySystem.Application.UseCases.OrderItem.Update;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Dto.Pagination;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KitchenDeliverySystem.Test.Integration.Presentation
{
    public class OrdersControllerTests
    {
        private readonly Mock<ICreateOrderUseCase> _mockCreateOrderUseCase;
        private readonly Mock<IDeleteOrderUseCase> _mockDeleteOrderUseCase;
        private readonly Mock<IGetOrderUseCase> _mockGetOrderUseCase;
        private readonly Mock<IUpdateOrderUseCase> _mockUpdateOrderUseCase;
        private readonly Mock<ISearchOrderUseCase> _mockSearchOrderUseCase;
        private readonly Mock<ICreateOrderItemUseCase> _mockCreateOrderItemUseCase;
        private readonly Mock<IUpdateOrderItemUseCase> _mockUpdateOrderItemUseCase;
        private readonly Mock<IOrdemItemDeleteUseCase> _mockDeleteOrderItemUseCase;
        private readonly Mock<IGetOrderItemUseCase> _mockGetOrderItemUseCase;
        private readonly OrdersController _controller;
        private readonly Faker<OrderDto> _orderFaker;
        private readonly Faker<CreateOderDto> _createOrderFaker;
        private readonly Faker<UpdateOrderDto> _updateOrderFaker;
        private readonly Faker<OrderItemDto> _orderItemFaker;
        private readonly Faker<CreateOrderItemDto> _createOrderItemFaker;
        private readonly Faker<UpdateOrderItemDto> _updateOrderItemFaker;

        public OrdersControllerTests()
        {
            _mockCreateOrderUseCase = new Mock<ICreateOrderUseCase>();
            _mockDeleteOrderUseCase = new Mock<IDeleteOrderUseCase>();
            _mockGetOrderUseCase = new Mock<IGetOrderUseCase>();
            _mockUpdateOrderUseCase = new Mock<IUpdateOrderUseCase>();
            _mockSearchOrderUseCase = new Mock<ISearchOrderUseCase>();
            _mockCreateOrderItemUseCase = new Mock<ICreateOrderItemUseCase>();
            _mockUpdateOrderItemUseCase = new Mock<IUpdateOrderItemUseCase>();
            _mockDeleteOrderItemUseCase = new Mock<IOrdemItemDeleteUseCase>();
            _mockGetOrderItemUseCase = new Mock<IGetOrderItemUseCase>();

            _controller = new OrdersController(
                _mockCreateOrderUseCase.Object,
                _mockDeleteOrderUseCase.Object,
                _mockGetOrderUseCase.Object,
                _mockUpdateOrderUseCase.Object,
                _mockSearchOrderUseCase.Object,
                _mockCreateOrderItemUseCase.Object,
                _mockUpdateOrderItemUseCase.Object,
                _mockDeleteOrderItemUseCase.Object,
                _mockGetOrderItemUseCase.Object);

            _orderFaker = new Faker<OrderDto>()
                .RuleFor(o => o.CustomerName, f => f.Name.FullName())
                .RuleFor(o => o.OrderTime, f => f.Date.Recent())
                .RuleFor(o => o.OrderStatus, f => f.PickRandom<OrderStatus>());

            _createOrderFaker = new Faker<CreateOderDto>()
                .RuleFor(o => o.CustomerName, f => f.Name.FullName());

            _updateOrderFaker = new Faker<UpdateOrderDto>()
                .RuleFor(o => o.CustomerName, f => f.Name.FullName())
                .RuleFor(o => o.OrderStatus, f => f.PickRandom<OrderStatus>());

            _orderItemFaker = new Faker<OrderItemDto>()
                .RuleFor(oi => oi.Name, f => f.Commerce.ProductName())
                .RuleFor(oi => oi.Quantity, f => f.Random.Decimal(1, 100))
                .RuleFor(oi => oi.Notes, f => f.Lorem.Sentence());

            _createOrderItemFaker = new Faker<CreateOrderItemDto>()
                .RuleFor(oi => oi.Name, f => f.Commerce.ProductName())
                .RuleFor(oi => oi.Quantity, f => f.Random.Decimal(1, 100))
                .RuleFor(oi => oi.Notes, f => f.Lorem.Sentence());

            _updateOrderItemFaker = new Faker<UpdateOrderItemDto>()
                .RuleFor(oi => oi.Name, f => f.Commerce.ProductName())
                .RuleFor(oi => oi.Quantity, f => f.Random.Decimal(1, 100))
                .RuleFor(oi => oi.Notes, f => f.Lorem.Sentence());
        }

        [Fact]
        public async Task GetOrders_ReturnsOkResult()
        {
            // Arrange
            var orderFilterDto = new OrderFilterDto();
            var pagedResult = new PagedResultDto<OrderDto>
            {
                Data = _orderFaker.Generate(10),
                PageNumber = 1,
                PageSize = 10,
                Total = 10
            };
            _mockSearchOrderUseCase.Setup(x => x.ExecuteAsync(It.IsAny<OrderFilterDto>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetOrders(orderFilterDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<PagedResultDto<OrderDto>>(okResult.Value);
            _mockSearchOrderUseCase.Verify(x => x.ExecuteAsync(It.IsAny<OrderFilterDto>()), Times.Once);
        }

        [Fact]
        public async Task GetOrders_NoOrdersFound_ReturnsNotFoundResult()
        {
            // Arrange
            var orderFilterDto = new OrderFilterDto();
            var pagedResult = new PagedResultDto<OrderDto>
            {
                Data = new List<OrderDto>(),
                PageNumber = 1,
                PageSize = 10,
                Total = 0
            };
            _mockSearchOrderUseCase.Setup(x => x.ExecuteAsync(It.IsAny<OrderFilterDto>()))
                .ReturnsAsync(ErrorCatalog.OrderNotFound);

            // Act
            var result = await _controller.GetOrders(orderFilterDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var order = _orderFaker.Generate();
            _mockGetOrderUseCase.Setup(x => x.ExecuteAsync(orderId))
                .ReturnsAsync(ErrorCatalog.OrderNotFound);

            // Act
            var result = await _controller.GetOrderById(orderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderById_NoOrderFound_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            var order = _orderFaker.Generate();
            _mockGetOrderUseCase.Setup(x => x.ExecuteAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrderById(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<OrderDto>(okResult.Value);
            _mockGetOrderUseCase.Verify(x => x.ExecuteAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedResult()
        {
            // Arrange
            var createOrderDto = _createOrderFaker.Generate();
            var createdOrder = _orderFaker.Generate();
            _mockCreateOrderUseCase.Setup(x => x.ExecuteAsync(It.IsAny<CreateOderDto>()))
                .ReturnsAsync(createdOrder);

            // Act
            var result = await _controller.CreateOrder(createOrderDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.IsType<OrderDto>(createdResult.Value);
            _mockCreateOrderUseCase.Verify(x => x.ExecuteAsync(It.IsAny<CreateOderDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_InvalidData_ReturnsUnprocessableEntity()
        {
            // Arrange
            var createOrderDto = _createOrderFaker.Clone()
                .RuleFor(x => x.CustomerName, x => string.Empty)
                .Generate();

            var createdOrder = _orderFaker.Generate();
            _mockCreateOrderUseCase.Setup(x => x.ExecuteAsync(It.IsAny<CreateOderDto>()))
                .ReturnsAsync(createdOrder);

            // Act
            var result = await _controller.CreateOrder(createOrderDto);

            // Assert
            var unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var updateOrderDto = _updateOrderFaker.Generate();
            var updatedOrder = _orderFaker.Generate();
            _mockUpdateOrderUseCase.Setup(x => x.ExecuteAsync(orderId, It.IsAny<UpdateOrderDto>()))
                .ReturnsAsync(updatedOrder);

            // Act
            var result = await _controller.UpdateOrder(orderId, updateOrderDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<OrderDto>(okResult.Value);
            _mockUpdateOrderUseCase.Verify(x => x.ExecuteAsync(orderId, It.IsAny<UpdateOrderDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrder_InvalidData_ReturnsUnprocessableEntity()
        {
            // Arrange
            var orderId = 1;
            var updateOrderDto = _updateOrderFaker.Clone()
                .RuleFor(x => x.CustomerName, string.Empty);

            var updatedOrder = _orderFaker.Generate();
            _mockUpdateOrderUseCase.Setup(x => x.ExecuteAsync(orderId, It.IsAny<UpdateOrderDto>()))
                .ReturnsAsync(updatedOrder);

            // Act
            var result = await _controller.UpdateOrder(orderId, updateOrderDto);

            // Assert
            var unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            _mockDeleteOrderUseCase.Setup(x => x.ExecuteAsync(orderId))
                .ReturnsAsync(new Success());

            // Act
            var result = await _controller.DeleteOrder(orderId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockDeleteOrderUseCase.Verify(x => x.ExecuteAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task DeleteOrder_OrderNotFoun_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            _mockDeleteOrderUseCase.Setup(x => x.ExecuteAsync(orderId))
                .ReturnsAsync(ErrorCatalog.OrderNotFound);

            // Act
            var result = await _controller.DeleteOrder(orderId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderItems_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var orderItems = _orderItemFaker.Generate(5);
            _mockGetOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId))
                .ReturnsAsync(orderItems);

            // Act
            var result = await _controller.GetOrderItems(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<List<OrderItemDto>>(okResult.Value);
            _mockGetOrderItemUseCase.Verify(x => x.ExecuteAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetOrderItems_OrderNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            _mockGetOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId))
                .ReturnsAsync(ErrorCatalog.OrderNotFound);

            // Act
            var result = await _controller.GetOrderItems(orderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderItems_OrderItemsNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            _mockGetOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId))
                .ReturnsAsync(ErrorCatalog.OrderItemsNotFound);

            // Act
            var result = await _controller.GetOrderItems(orderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task AddOrderItem_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var createOrderItemDto = _createOrderItemFaker.Generate();
            var createdOrderItem = _orderItemFaker.Generate();
            _mockCreateOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, It.IsAny<CreateOrderItemDto>()))
                .ReturnsAsync(createdOrderItem);

            // Act
            var result = await _controller.AddOrderItem(orderId, createOrderItemDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<OrderItemDto>(okResult.Value);
            _mockCreateOrderItemUseCase.Verify(x => x.ExecuteAsync(orderId, It.IsAny<CreateOrderItemDto>()), Times.Once);
        }

        [Fact]
        public async Task AddOrderItem_InvalidData_ReturnsUnprocessableEntity()
        {
            // Arrange
            var orderId = 1;
            var createOrderItemDto = _createOrderItemFaker.Clone()
                .RuleFor(x => x.Quantity, 0);

            var createdOrderItem = _orderItemFaker.Generate();
            _mockCreateOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, It.IsAny<CreateOrderItemDto>()))
                .ReturnsAsync(createdOrderItem);

            // Act
            var result = await _controller.AddOrderItem(orderId, createOrderItemDto);

            // Assert
            var unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result.Result);
        }

        [Fact]
        public async Task AddOrderItem_OrderNotFound_ReturnsUnprocessableEntity()
        {
            // Arrange
            var orderId = 1;
            var createOrderItemDto = _createOrderItemFaker.Clone()
                .RuleFor(x => x.Quantity, 0);

            var createdOrderItem = _orderItemFaker.Generate();
            _mockCreateOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, It.IsAny<CreateOrderItemDto>()))
                .ReturnsAsync(createdOrderItem);

            // Act
            var result = await _controller.AddOrderItem(orderId, createOrderItemDto);

            // Assert
            var unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateOrderItem_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            var updateOrderItemDto = _updateOrderItemFaker.Generate();
            var updatedOrderItem = _orderItemFaker.Generate();
            _mockUpdateOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, itemId, It.IsAny<UpdateOrderItemDto>()))
                .ReturnsAsync(updatedOrderItem);

            // Act
            var result = await _controller.UpdateOrderItem(orderId, itemId, updateOrderItemDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<OrderItemDto>(okResult.Value);
            _mockUpdateOrderItemUseCase.Verify(x => x.ExecuteAsync(orderId, itemId, It.IsAny<UpdateOrderItemDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderItem_InvalidData_ReturnsUnprocessableEntity()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            var updateOrderItemDto = _updateOrderItemFaker.Clone()
                .RuleFor(x => x.Quantity, 0);

            var updatedOrderItem = _orderItemFaker.Generate();
            _mockUpdateOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, itemId, It.IsAny<UpdateOrderItemDto>()))
                .ReturnsAsync(updatedOrderItem);

            // Act
            var result = await _controller.UpdateOrderItem(orderId, itemId, updateOrderItemDto);

            // Assert
            var unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateOrderItem_OrderNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            var updateOrderItemDto = _updateOrderItemFaker.Generate();

            var updatedOrderItem = _orderItemFaker.Generate();
            _mockUpdateOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, itemId, It.IsAny<UpdateOrderItemDto>()))
                .ReturnsAsync(ErrorCatalog.OrderNotFound);

            // Act
            var result = await _controller.UpdateOrderItem(orderId, itemId, updateOrderItemDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteOrderItem_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            _mockDeleteOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, itemId))
                .ReturnsAsync(new Success());

            // Act
            var result = await _controller.DeleteOrderItem(orderId, itemId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockDeleteOrderItemUseCase.Verify(x => x.ExecuteAsync(orderId, itemId), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderItem_OrderNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            _mockDeleteOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, itemId))
                .ReturnsAsync(ErrorCatalog.OrderNotFound);

            // Act
            var result = await _controller.DeleteOrderItem(orderId, itemId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteOrderItem_OrderItemNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            var itemId = 1;
            _mockDeleteOrderItemUseCase.Setup(x => x.ExecuteAsync(orderId, itemId))
                .ReturnsAsync(ErrorCatalog.OrderItemNotFound);

            // Act
            var result = await _controller.DeleteOrderItem(orderId, itemId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
