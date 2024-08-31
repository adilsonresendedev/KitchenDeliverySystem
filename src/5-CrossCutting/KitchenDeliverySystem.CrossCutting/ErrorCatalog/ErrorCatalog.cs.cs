using ErrorOr;

namespace KitchenDeliverySystem.CrossCutting.ErrorCatalog
{
    public static class ErrorCatalog
    {
        public static Error UserNotFound => Error.NotFound("ERR-01", "User not found.");
        public static Error UserInvalidPassword => Error.Validation("ERR-02", "Invalid password.");
        public static Error UserAlterdyExists => Error.Validation("ERR-03", "Usernae already exists.");
        public static Error OrderNotFound => Error.NotFound("ERR-04", "Order not found.");
        public static Error OrderCantDeleteHasItems => Error.Validation("ERR-05", "Order can't be deleted because it has items.");
        public static Error OrderItemNotFound => Error.Validation("ERR-06", "Order item not found.");
        public static Error OrderItemsNotFound => Error.Validation("ERR-06", "Order items not found.");
    }
}
