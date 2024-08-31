using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Dto.Base;

namespace KitchenDeliverySystem.Dto.Order
{
    public class OrderFilterDto : BaseFilterDto
    {
        public string CustomerName { get; set; }
        public DateTime? OrderTimeStart { get; set; }
        public DateTime? OrderTimeEnd { get; set; }
        public OrderStatus? OrderStatus { get; set; }
    }
}
