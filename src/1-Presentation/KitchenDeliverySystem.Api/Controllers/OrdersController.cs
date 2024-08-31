using KitchenDeliverySystem.Dto.Order;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDeliverySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // GET /orders - Retrieves a list of orders
        [HttpGet]
        public async Task<ActionResult<List<UpdateOrderDto>>> GetOrders()
        {
            return Ok();
        }

        // GET /orders/{id} - Retrieves a specific order by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UpdateOrderDto>> GetOrderById(int id)
        {
            return Ok();
        }

        // POST /orders - Adds a new order
        [HttpPost]
        public async Task<ActionResult<UpdateOrderDto>> CreateOrder([FromBody] UpdateOrderDto createOrderDto)
        {
            return Ok();
        }

        // PUT /orders/{id} - Updates an existing order
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            return Ok();
        }

        // DELETE /orders/{id} - Removes an order
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return Ok();
        }

        // GET /orders/{id}/items - Retrieves items of a specific order
        [HttpGet("{id:int}/items")]
        public async Task<ActionResult<List<OrderItemDto>>> GetOrderItems(int id)
        {
            return Ok();
        }

        // POST /orders/{id}/items - Adds a new item to an order
        [HttpPost("{id:int}/items")]
        public async Task<ActionResult<OrderItemDto>> AddOrderItem(int id, [FromBody] CreateOrderItemDto addOrderItemDto)
        {
            return Ok();
        }

        // PUT /orders/{id}/items/{itemId} - Updates an item in an order
        [HttpPut("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> UpdateOrderItem(int id, int itemId, [FromBody] CreateOrderItemDto updateOrderItemDto)
        {
            return Ok();
        }

        // DELETE /orders/{id}/items/{itemId} - Removes an item from an order
        [HttpDelete("{id:int}/items/{itemId:int}")]
        public async Task<IActionResult> DeleteOrderItem(int id, int itemId)
        {
            return Ok();
        }
    }
}
