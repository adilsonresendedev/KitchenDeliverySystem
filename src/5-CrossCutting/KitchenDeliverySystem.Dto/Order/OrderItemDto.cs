using KitchenDeliverySystem.Dto.Base;

namespace KitchenDeliverySystem.Dto.Order
{
    public class OrderItemDto : BaseDto
    {
        public string Name { get; private set; }
        public decimal Quantity { get; private set; }
        public string Notes { get; private set; }

    }
}
