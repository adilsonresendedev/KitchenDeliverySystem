namespace KitchenDeliverySystem.Dto.User
{
    public class CreateUserDto : UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
