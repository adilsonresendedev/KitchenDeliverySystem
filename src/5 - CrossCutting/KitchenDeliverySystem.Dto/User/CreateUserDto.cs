namespace KitchenDeliverySystem.Dto.User
{
    public class CreateUserDto : UserLoginDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
