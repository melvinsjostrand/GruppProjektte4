using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
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
            if (user.Role != 1)
            {
                return StatusCode(403, "Du får inte lägga saker i en kundvagn");
            }
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO `cart` (`amount`, `productId`, `userId`) VALUES('1', @Id, @userId)";
                query.Parameters.AddWithValue("@Id", product.Id);
                query.Parameters.AddWithValue("@userId", user.Id);
                int row = query.ExecuteNonQuery();
            }catch(Exception ex)
            {
                connection.Close();
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201);
        }

        [HttpPut]
        public ActionResult ChangeAmount(cart cart)
        {
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "UPDATE cart " + "SET Amount = @amount " + "WHERE cartId = @cartId";
                query.Parameters.AddWithValue("@amount", cart.Amount);
                query.Parameters.AddWithValue("@cartId", cart.cartId);
                int row = query.ExecuteNonQuery();
            }catch(Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "antal ändrad");
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
    public ActionResult<cart> GetCart()
        {
            string auth = Request.Headers["Authorization"];//GUID
            if (auth == null || !LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "du är inte inloggad");
            }

            User user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
            List<cart> Cart = new List<cart>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT cartId, amount, userId, price, t2.Name, t1.productId FROM cart t1 LEFT JOIN product t2 ON t1.productId = t2.Id WHERE userId = @Id";
                query.Parameters.AddWithValue("@Id", user.Id);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read()) 
                {
                    cart carts = new cart
                    {
                        cartId = data.GetInt32("cartId"),
                        Amount = data.GetInt32("Amount"),
                        productId = data.GetInt32("productId"),
                        userId = data.GetInt32("userId"),
                        Name = data.GetString("Name"),
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
