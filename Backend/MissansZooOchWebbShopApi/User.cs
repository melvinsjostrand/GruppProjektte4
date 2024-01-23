using MissansZooOchWebbShopApi.Controllers;

namespace MissansZooOchWebbShopApi
{
    public class User
    {
        public int Id { get; set; }
        public int Role { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
    }
}
