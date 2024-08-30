namespace KitchenDeliverySystem.Dto.Order
{
    public class CreateOrderItemDto
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Notes { get; set; }
    }
}
