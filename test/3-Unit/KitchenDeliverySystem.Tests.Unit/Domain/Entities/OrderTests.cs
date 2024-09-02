using KitchenDeliverySystem.Domain.Entities;
using KitchenDeliverySystem.Domain.Enums;

namespace KitchenDeliverySystem.Test.Unit.Domain.Entities
{
    public class OrderTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties()
        {
            // Arrange
            string customerName = "John Doe";

            // Act
            var order = new Order(customerName);

            // Assert
            Assert.Equal(customerName, order.CustomerName);
            Assert.Equal(OrderStatus.Pending, order.OrderStatus);
            Assert.Empty(order.Items);
        }

        // Test Update Method
        [Fact]
        public void Update_Should_Modify_Properties()
        {
            // Arrange
            var order = new Order("Initial Customer");
            var newCustomerName = "Updated Customer";
            var newOrderStatus = OrderStatus.Completed;

            // Act
            order.Update(newCustomerName, newOrderStatus);

            // Assert
            Assert.Equal(newCustomerName, order.CustomerName);
            Assert.Equal(newOrderStatus, order.OrderStatus);
        }

        [Fact]
        public void AddItem_Should_Add_Item_To_List()
        {
            // Arrange
            var order = new Order("Customer");
            var orderItem = new OrderItem(1, "Item1", 2.0m, "Notes");

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Contains(orderItem, order.Items);
        }

        [Fact]
        public void RemoveItem_Should_Remove_Item_From_List()
        {
            // Arrange
            var order = new Order("Customer");
            var orderItem = new OrderItem(1, "Item1", 2.0m, "Notes");
            order.AddItem(orderItem);

            // Act
            order.RemoveItem(orderItem);

            // Assert
            Assert.DoesNotContain(orderItem, order.Items);
        }
    }
}
