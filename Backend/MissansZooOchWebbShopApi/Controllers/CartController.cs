using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("Cart")]
    [ApiController]
    public class CartController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");

        [HttpPost]//ladda upp till cart
        public ActionResult AddToCart(cart cart)
        {
            Product product = new Product();
            User user = new User();
            string auth = Request.Headers["Authorization"];//GUID
            if (auth == null || !LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "du är inte inloggad");
            }

            user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
            if (user.Role != 2)
            {
                return StatusCode(403, "Du har inte rätten till att skapa produkter");
            }
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO cart (productId, userId, Amount ) " + "(@productId, userId, 1)";
                query.Parameters.AddWithValue("@productId", product.Id);
                query.Parameters.AddWithValue("@userId", user.UserId);
            }catch(Exception ex)
            {
                connection.Close();
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201);
        }
    }
}
