namespace KitchenDeliverySystem.CrossCutting.Options
{
    public class AppSettings
    {
        public string TokenKey { get; set; }
        public ConnectionStringsSettings ConnectionStrings { get; set; }
    }

    public class ConnectionStringsSettings
    {
        public string DefaultConnection { get; set; }
    }
}
