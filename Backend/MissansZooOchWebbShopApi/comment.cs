namespace MissansZooOchWebbShopApi
{
    public class Comment
    {
        public int Id { get; set; }
        public string text { get; set; } = string.Empty;
        public int blogId { get; set; }
        public int userId { get; set; }
        public string username { get; set; } = string.Empty;
    }
}
