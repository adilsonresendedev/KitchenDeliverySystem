using ErrorOr;

namespace KitchenDeliverySystem.CrossCutting.ErrorCatalog
{
    public static class ErrorCatalog
    {
        public static Error UserNotFound => Error.NotFound("User not found.");
        public static Error UserInvalidPassword => Error.Validation("Invalid password.");
        public static Error UserAlterdyExists => Error.Validation("Usernae already exists.");
    }
}
