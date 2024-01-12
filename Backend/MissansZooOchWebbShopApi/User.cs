using MissansZooOchWebbShopApi.Controllers;

namespace MissansZooOchWebbShopApi
{
    public class User
    {
        public int UserId { get; set; } = 1;
        public int Role { get; set; } = 1;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
    }
}
