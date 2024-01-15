namespace MissansZooOchWebbShopApi
{
    public class cart
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int productId { get; set; }
        public int userId { get; set; }
        public string productName { get; set; } = string.Empty;
        public int price { get; set; }
    }
}
