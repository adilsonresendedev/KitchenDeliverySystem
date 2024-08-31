using KitchenDeliverySystem.Api.Controllers.Base;
using KitchenDeliverySystem.Api.Validation;
using KitchenDeliverySystem.Application.UseCases.Order.OrderCreate;
using KitchenDeliverySystem.Application.UseCases.Order.OrderDelete;
using KitchenDeliverySystem.Application.UseCases.Order.OrderGet;
using KitchenDeliverySystem.Application.UseCases.Order.OrderSearch;
using KitchenDeliverySystem.Application.UseCases.Order.OrderUpdate;
using KitchenDeliverySystem.Dto.Order;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDeliverySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        private readonly ICreateOrderUseCase _createOrderUseCase;
        private readonly IDeleteOrderUseCase _deleteOrderUseCase;
        private readonly IGetOrderUseCase _getOrderUseCase;
        private readonly IUpdateOrderUseCase _updateOrderUseCase;
        private readonly ISearchOrderUseCase _searchOrderUseCase;

        public OrdersController(
            ICreateOrderUseCase createOrderUseCase,
            IDeleteOrderUseCase deleteOrderUseCase,
            IGetOrderUseCase getOrderUseCase,
            IUpdateOrderUseCase updateOrderUseCase,
            ISearchOrderUseCase searchOrderUseCase)
        {
            _createOrderUseCase = createOrderUseCase;
            _deleteOrderUseCase = deleteOrderUseCase;
            _getOrderUseCase = getOrderUseCase;
            _updateOrderUseCase = updateOrderUseCase;
            _searchOrderUseCase = searchOrderUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<List<UpdateOrderDto>>> GetOrders([FromQuery] OrderFilterDto orderFilterDto)
        {
            var result = await _searchOrderUseCase.ExecuteAsync(orderFilterDto);
            return HandleErrorOrResult(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UpdateOrderDto>> GetOrderById(int id)
        {
            var result = await _getOrderUseCase.ExecuteAsync(id);
            return HandleErrorOrResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOderDto createOrderDto)
        {
            var createOrderValidator = new CreateOrderDtoValidator();
            var validationResult = createOrderValidator.Validate(createOrderDto);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            var result = await _createOrderUseCase.ExecuteAsync(createOrderDto);
            return HandleErrorOrResult(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            var createOrderValidator = new UpdateOrderDtoValidator();
            var validationResult = createOrderValidator.Validate(updateOrderDto);
            if (!validationResult.IsValid)
                return UnprocessableEntity(validationResult);

            var result = await _updateOrderUseCase.ExecuteAsync(id, updateOrderDto);
            return HandleErrorOrResult(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _deleteOrderUseCase.ExecuteAsync(id);
            return HandleErrorOrResult(result);
        }

        [HttpGet("{id:int}/items")]
        public async Task<ActionResult<List<OrderItemDto>>> GetOrderItems(int id)
        {
            return Ok();
        }

        [HttpPost("{id:int}/items")]
        public async Task<ActionResult<OrderItemDto>> AddOrderItem(int id, [FromBody] CreateOrderItemDto addOrderItemDto)
        {
            return Ok();
        }

        [HttpPut("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> UpdateOrderItem(int id, int itemId, [FromBody] CreateOrderItemDto updateOrderItemDto)
        {
            return Ok();
        }

        [HttpDelete("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> DeleteOrderItem(int id, int itemId)
        {
            return Ok();
        }
    }
}
