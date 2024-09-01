using KitchenDeliverySystem.Domain.Enums;

namespace KitchenDeliverySystem.Dto.Order
{
    public class UpdateOrderDto : CreateOrderDto
    {
        public OrderStatus OrderStatus { get; set; }
    }
}
