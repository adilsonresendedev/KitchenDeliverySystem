namespace KitchenDeliverySystem.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        protected OrderItem() { }
        public string Name { get; private set; }
        public decimal Quantity { get; private set; }
        public string Notes { get; private set; }

        public OrderItem(string name, decimal quantity, string notes)
        {
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
    }
}
