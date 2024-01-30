namespace MissansZooOchWebbShopApi
{
    public class Comment
    {
        public int commentId { get; set; }
        public string commentText { get; set; } = string.Empty;
        public int blogId { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
    }
}
