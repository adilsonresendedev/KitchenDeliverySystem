using KitchenDeliverySystem.Domain.Enums;

namespace KitchenDeliverySystem.Dto.Order
{
    public class CreateOderDto
    {
        public string CustomerName { get; private set; }
        public DateTime OrderTime { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
    }
}
