using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Reflection.Metadata;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("Cart")]
    [ApiController]
    public class CartController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");

        [HttpPost]//ladda upp till cart
        public ActionResult AddToCart(Product product)
        {

            string auth = Request.Headers["Authorization"];//GUID
            if (auth == null || !LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "du är inte inloggad");
            }

            User user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
            if (user.Role != 2)
            {
                return StatusCode(403, "Du har inte rätten till att skapa produkter");
            }
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO cart (Id, userId, Amount ) " + "(@Id, @userId, 1)";
                query.Parameters.AddWithValue("@Id", product.Id);
                query.Parameters.AddWithValue("@userId", user.Id);
            }catch(Exception ex)
            {
                connection.Close();
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201);
        }
        [HttpDelete]
        public ActionResult DeleteCart(cart cart)
        {
            User user = new User();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "DELETE FROM cart WHERE userId = @userId";
                query.Parameters.AddWithValue("@userId", user.Id);
                int row = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "gick inte att ta bort");
            }
            return StatusCode(200, "blogg har tagits bort");
        }
    [HttpGet]
    public ActionResult<cart> GetCart(int Id)
        {
            List<cart> Cart = new List<cart>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT cartId, amount, userId, price, productName, t1.productId FROM cart t1 LEFT JOIN product t2 ON t1.productId = t2.productId WHERE userId = @Id";
                query.Parameters.AddWithValue("@Id", Id);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read()) 
                {
                    cart carts = new cart
                    {
                        cartId = data.GetInt32("cartId"),
                        Amount = data.GetInt32("Amount"),
                        productId = data.GetInt32("productId"),
                        userId = data.GetInt32("userId"),
                        productName = data.GetString("productName"),
                        price = data.GetInt32("price")
                    };
                    Cart.Add(carts);
                }
            }catch(Exception ex) 
            {
                return StatusCode(500, "something went wrong");
            }
            return Ok(Cart);
        }
    }

}
