using KitchenDeliverySystem.Domain.Enums;

namespace KitchenDeliverySystem.Dto.Order
{
    public class UpdateOrderDto : CreateOderDto
    {
        public OrderStatus OrderStatus { get; set; }
    }
}
