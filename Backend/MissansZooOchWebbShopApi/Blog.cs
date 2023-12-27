namespace MissansZooOchWebbShopApi
{
    public class Blog
    {
        public int blogId { get; set; }
        public int userId { get; set; }
        public string title { get; set; } = string.Empty;
        public string blogImg { get; set; } = string.Empty;
        public string blogText { get; set; } = string.Empty;    
    }
}
