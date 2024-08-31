using KitchenDeliverySystem.Domain.Enums;

namespace KitchenDeliverySystem.Domain.Filters
{
    public class OrderFilter : BaseFilter
    {
        public string CustomerName { get; set; }
        public DateTime? OrderTimeStart { get; set; }
        public DateTime? OrderTimeEnd { get; set; }
        public OrderStatus? OrderStatus { get; set; }
    }
}
