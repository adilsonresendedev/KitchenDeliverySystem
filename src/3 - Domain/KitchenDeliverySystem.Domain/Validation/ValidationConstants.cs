namespace KitchenDeliverySystem.Domain.Validation
{
    public static class ValidationConstants
    {
        public static string FirstNameIsInvalid = "FirstName is invalid.";
        public static string LastNameIsInvalid = "LastName is invalid.";
        public static string UserNameIsInvalid = "UserName is invalid.";
        public static string PasswordHashIsInvalid = "Password hash is invalid.";
        public static string PasswordSaltIsInvalid = "Password salt is invalid.";

        public static string CustomerNameIsInvalid = "Customer name is invalid.";
        public static string CustomerNameTooLong = "Customer name cannot exceed 30 characters.";

        public static string OrderStatusIsInvalid = "Invalid order status.";
        public static string OrderItemNameIsInvalid = "Order item name is invalid.";
        public static string OrderItemNameTooLong = "Order item name cannot exceed 30 characters.";
        public static string OrderItemQuantityIsInvalid = "Order item quantity must be greater than zero.";
        public static string OrderItemNotesIsInvalid = "Notes should not exceed 200 characters.";
    }
}
