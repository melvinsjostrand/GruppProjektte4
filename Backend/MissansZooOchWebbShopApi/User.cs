using MissansZooOchWebbShopApi.Controllers;

namespace MissansZooOchWebbShopApi
{
    public class User
    {
        public int UserId { get; set; }
        public int Role { get; set; } = 1;
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

        public string Mail { get; set; } = String.Empty;

    }
}
