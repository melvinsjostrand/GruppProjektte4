namespace MissansZooOchWebbShopApi
{
    public class cart
    {
        public int cartId { get; set; }
        public int Amount { get; set; }
        public int productId { get; set; }
        public int userId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int price { get; set; }
    }
}
