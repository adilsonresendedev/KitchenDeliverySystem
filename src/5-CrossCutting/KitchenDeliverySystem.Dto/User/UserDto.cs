using KitchenDeliverySystem.Dto.Base;

namespace KitchenDeliverySystem.Dto.User
{
    public class UserDto : BaseDto
    {
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get;  set; }
        public string Username { get; set; }
    }
}
