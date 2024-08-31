using KitchenDeliverySystem.Api.Controllers.Base;
using KitchenDeliverySystem.Api.Validation;
using KitchenDeliverySystem.Application.UseCases.Order.OrderCreate;
using KitchenDeliverySystem.Application.UseCases.Order.OrderDelete;
using KitchenDeliverySystem.Application.UseCases.Order.OrderGet;
using KitchenDeliverySystem.Application.UseCases.Order.OrderSearch;
using KitchenDeliverySystem.Application.UseCases.Order.OrderUpdate;
using KitchenDeliverySystem.Application.UseCases.OrderItem.Get;
using KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemCreate;
using KitchenDeliverySystem.Application.UseCases.OrderItem.OrderItemDelete;
using KitchenDeliverySystem.Application.UseCases.OrderItem.Update;
using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Dto.Order;
using KitchenDeliverySystem.Dto.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDeliverySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly ICreateOrderUseCase _createOrderUseCase;
        private readonly IDeleteOrderUseCase _deleteOrderUseCase;
        private readonly IGetOrderUseCase _getOrderUseCase;
        private readonly IUpdateOrderUseCase _updateOrderUseCase;
        private readonly ISearchOrderUseCase _searchOrderUseCase;
        private readonly ICreateOrderItemUseCase _createOrderItemUseCase;
        private readonly IUpdateOrderItemUseCase _updateOrderItemUseCase;
        private readonly IOrdemItemDeleteUseCase _deleteOrderItemUseCase;
        private readonly IGetOrderItemUseCase _getOrderItemUseCase;

        public OrdersController(
            ICreateOrderUseCase createOrderUseCase,
            IDeleteOrderUseCase deleteOrderUseCase,
            IGetOrderUseCase getOrderUseCase,
            IUpdateOrderUseCase updateOrderUseCase,
            ISearchOrderUseCase searchOrderUseCase,
            ICreateOrderItemUseCase createOrderItemUseCase,
            IUpdateOrderItemUseCase updateOrderItemUseCase,
            IOrdemItemDeleteUseCase deleteOrderItemUseCase,
            IGetOrderItemUseCase getOrderItemUseCase)
        {
            _createOrderUseCase = createOrderUseCase;
            _deleteOrderUseCase = deleteOrderUseCase;
            _getOrderUseCase = getOrderUseCase;
            _updateOrderUseCase = updateOrderUseCase;
            _searchOrderUseCase = searchOrderUseCase;
            _createOrderItemUseCase = createOrderItemUseCase;
            _updateOrderItemUseCase = updateOrderItemUseCase;
            _deleteOrderItemUseCase = deleteOrderItemUseCase;
            _getOrderItemUseCase = getOrderItemUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] OrderFilterDto inbound)
        {
            var result = await _searchOrderUseCase.ExecuteAsync(inbound);
            return HandleErrorOrResult(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateOrderDto>> GetOrderById(int id)
        {
            var result = await _getOrderUseCase.ExecuteAsync(id);
            return HandleErrorOrResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOderDto inbound)
        {
            var createOrderValidator = new CreateOrderDtoValidator();
            var validationResult = createOrderValidator.Validate(inbound);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            var result = await _createOrderUseCase.ExecuteAsync(inbound);
            return HandleErrorOrResult(result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, [FromBody] UpdateOrderDto inbound)
        {
            var createOrderValidator = new UpdateOrderDtoValidator();
            var validationResult = createOrderValidator.Validate(inbound);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            var result = await _updateOrderUseCase.ExecuteAsync(id, inbound);
            return HandleErrorOrResult(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _deleteOrderUseCase.ExecuteAsync(id);
            return HandleErrorOrResult(result);
        }

        [HttpGet("{id:int}/items")]
        [ProducesResponseType(typeof(List<OrderItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<OrderItemDto>>> GetOrderItems(int id)
        {
            var result = await _getOrderItemUseCase.ExecuteAsync(id);
            return HandleErrorOrResult(result);
        }

        [HttpPost("{id:int}/items")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<OrderItemDto>> AddOrderItem(int id, [FromBody] CreateOrderItemDto inbound)
        {
            var createOrderItemValidator = new CreateOrderItemDtoValidator();
            var validationResult = createOrderItemValidator.Validate(inbound);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            var result = await _createOrderItemUseCase.ExecuteAsync(id, inbound);
            return HandleErrorOrResult(result);
        }

        [HttpPut("{id:int}/items/{itemId:int}")]
        [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItemDto>> UpdateOrderItem(int id, int itemId, [FromBody] UpdateOrderItemDto inbound)
        {
            var updateOrderItemValidator = new UpdateOrderItemDtoValidator();
            var validationResult = updateOrderItemValidator.Validate(inbound);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            var result = await _updateOrderItemUseCase.ExecuteAsync(id, itemId, inbound);
            return HandleErrorOrResult(result);
        }

        [HttpDelete("{id:int}/items/{itemId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteOrderItem(int id, int itemId)
        {
            var result = await _deleteOrderItemUseCase.ExecuteAsync(id, itemId);
            return HandleErrorOrResult(result);
        }
    }
}
