using KitchenDeliverySystem.Domain.Entities;

namespace KitchenDeliverySystem.Test.Unit.Domain.Entities
{
    public class OrderItemTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties()
        {
            // Arrange
            int orderId = 1;
            string name = "Sample Item";
            decimal quantity = 10.5m;
            string notes = "Test notes";

            // Act
            var orderItem = new OrderItem(orderId, name, quantity, notes);

            // Assert
            Assert.Equal(orderId, orderItem.OrderId);
            Assert.Equal(name, orderItem.Name);
            Assert.Equal(quantity, orderItem.Quantity);
            Assert.Equal(notes, orderItem.Notes);
        }

        [Fact]
        public void Update_Should_Modify_Properties()
        {
            // Arrange
            var orderItem = new OrderItem(1, "Old Item", 5.0m, "Old notes");

            string newName = "Updated Item";
            decimal newQuantity = 15.0m;
            string newNotes = "Updated notes";

            // Act
            orderItem.Update(newName, newQuantity, newNotes);

            // Assert
            Assert.Equal(newName, orderItem.Name);
            Assert.Equal(newQuantity, orderItem.Quantity);
            Assert.Equal(newNotes, orderItem.Notes);
        }
    }
}
