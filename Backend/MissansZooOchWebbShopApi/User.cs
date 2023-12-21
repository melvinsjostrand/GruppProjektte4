using MissansZooOchWebbShopApi.Controllers;

namespace MissansZooOchWebbShopApi
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Login { get; set; } = String.Empty;
        public string Mail { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public LoginController.Roles Role { get; set; } = LoginController.Roles.Vanlig;
    }
}
