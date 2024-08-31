namespace KitchenDeliverySystem.Dto.Pagination
{
    public class PagedResultDto<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Total {  get; set; }
        public List<T> Data { get; set; }
    }
}
