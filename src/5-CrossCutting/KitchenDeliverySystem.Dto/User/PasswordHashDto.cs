namespace KitchenDeliverySystem.Dto.User
{
    public class PasswordHashDto
    {
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set;}
    }
}
