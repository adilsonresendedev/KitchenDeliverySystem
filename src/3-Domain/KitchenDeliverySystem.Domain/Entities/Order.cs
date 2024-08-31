using KitchenDeliverySystem.Domain.Enums;

namespace KitchenDeliverySystem.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string CustomerName { get; private set; }
        public DateTime OrderTime { get; private set; }
        public virtual List<OrderItem> Items { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        protected Order() { }

        public Order(string customerName)
        {          
            OrderTime = DateTime.UtcNow;
            CustomerName = customerName;
            OrderStatus = OrderStatus.Pending;
            Items = new List<OrderItem>();
        }

        public void Update(string customerName, OrderStatus orderStatus)
        {
            CustomerName = customerName;
            OrderStatus = orderStatus;
        }

        public void AddItem(OrderItem orderItem)
        {
            Items.Add(orderItem);
        }

        public void RemoveItem(OrderItem orderItem)
        {
            Items.Remove(orderItem);
        }
    }
}
