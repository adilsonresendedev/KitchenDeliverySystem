namespace KitchenDeliverySystem.Dto.Base
{
    public class BaseFilterDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
