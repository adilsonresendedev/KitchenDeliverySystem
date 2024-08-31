using KitchenDeliverySystem.Dto.Base;

namespace KitchenDeliverySystem.Dto.Order
{
    public class OrderItemDto : BaseDto
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Notes { get; set; }

    }
}
