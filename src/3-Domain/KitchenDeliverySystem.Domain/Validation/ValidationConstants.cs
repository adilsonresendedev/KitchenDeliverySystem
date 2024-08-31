namespace KitchenDeliverySystem.Domain.Validation
{
    public static class ValidationConstants
    {
        public static string FirstNameIsInvalid = "First name is required and must not exceed 50 characters.";
        public static string LastNameIsInvalid = "Last name is required and must not exceed 50 characters.";
        public static string UserNameIsInvalid = "Username is required and must not exceed 30 characters.";
        public static string PasswordIsInvalid = "Password is required and must be at least 8 characters long.";

        public static string CustomerNameIsInvalid = "Customer name is invalid.";
        public static string CustomerNameTooLong = "Customer name cannot exceed 30 characters.";

        public static string OrderStatusIsInvalid = "Invalid order status.";
        public static string OrderItemNameIsInvalid = "Order item name is invalid.";
        public static string OrderItemNameTooLong = "Order item name cannot exceed 30 characters.";
        public static string OrderItemQuantityIsInvalid = "Order item quantity must be greater than zero.";
        public static string OrderItemNotesIsInvalid = "Notes should not exceed 200 characters.";
    }
}
