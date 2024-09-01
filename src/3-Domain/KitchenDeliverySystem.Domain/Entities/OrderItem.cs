
namespace KitchenDeliverySystem.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        protected OrderItem() { }
        public int OrderId { get; private set; }
        public string Name { get; private set; }
        public decimal Quantity { get; private set; }
        public string Notes { get; private set; }

        public OrderItem(int orderId, string name, decimal quantity, string notes)
        {
            OrderId = orderId;
            Name = name;
            Quantity = quantity;
            Notes = notes;
        }

        public void Update(string name, decimal quantity, string notes)
        {
            Name = name;
            Quantity = quantity;
            Notes = notes;
        }

        public object Should()
        {
            throw new NotImplementedException();
        }
    }
}
