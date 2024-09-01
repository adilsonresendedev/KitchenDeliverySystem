using KitchenDeliverySystem.Dto.Base;

namespace KitchenDeliverySystem.Dto.Order
{
    public class OrderItemDto : BaseDto
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Notes { get; set; }

    }
}
