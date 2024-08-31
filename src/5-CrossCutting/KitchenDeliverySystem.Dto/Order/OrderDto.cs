using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Dto.Base;

namespace KitchenDeliverySystem.Dto.Order
{
    public class OrderDto : BaseDto
    {
        public string CustomerName { get; set; }
        public DateTime OrderTime { get; set; }
        public virtual List<OrderItemDto> Items { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
