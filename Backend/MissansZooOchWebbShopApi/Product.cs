namespace MissansZooOchWebbShopApi
{
    public class Product
    {
        public int Id { get; set; }
        public int price { get; set; }
        public string category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int stock { get; set; }
        public int rating { get; set; }
        public string content { get; set; } = string.Empty;
        public string feeding { get; set; } = string.Empty; 
    }
}
