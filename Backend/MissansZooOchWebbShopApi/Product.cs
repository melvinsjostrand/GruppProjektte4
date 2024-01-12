namespace MissansZooOchWebbShopApi
{
    public class Product
    {
        public int price { get; set; }
        public string category { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public string productImg { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int stock { get; set; }
        public int rating { get; set; }
        public int number { get; set; }
    }
}
