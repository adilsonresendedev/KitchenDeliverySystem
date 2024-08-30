using KitchenDeliverySystem.Domain.Enums;
using KitchenDeliverySystem.Domain.Validation;

namespace KitchenDeliverySystem.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string CustomerName { get; private set; }
        public DateTime OrderTime { get; private set; }
        public virtual List<OrderItem> Items { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        protected Order() { }

        public Order(string customerName, OrderStatus orderStatus)
        {          
            ValidateOrder(customerName, orderStatus);
            this.OrderTime = DateTime.UtcNow;
            Items = new List<OrderItem>();
        }

        public void Update(string customerName, List<OrderItem> items, OrderStatus orderStatus)
        {
            ValidateOrder(customerName,  orderStatus);
        }

        public void AddItem(OrderItem orderItem)
        {
            Items.Add(orderItem);
        }

        public void RemoveItem(OrderItem orderItem)
        {
            Items.Remove(orderItem);
        }

        public void ValidateOrder(string customerName, OrderStatus orderStatus)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(customerName), ValidationConstants.CustomerNameIsInvalid);

            DomainExceptionValidation.When(customerName.Length > 30, ValidationConstants.CustomerNameTooLong);

            CustomerName = customerName;
            OrderStatus = orderStatus;
        }
    }
}
