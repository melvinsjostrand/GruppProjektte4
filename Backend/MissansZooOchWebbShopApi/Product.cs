namespace MissansZooOchWebbShopApi
{
    public class Product
    {
        public int productId {  get; set; }
        public int price { get; set; }
        public string category { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public string productImg { get; set; } = string.Empty;

    }
}
