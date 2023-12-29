using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Reflection.Metadata;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");

        [HttpPost]
        public ActionResult CreateProduct(Product product)
        {
            User user = new User();
            string auth = Request.Headers["Authorization"];//GUID
            if (auth == null || LoginController.sessionId.ContainsKey(auth))
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
                query.CommandText = "INSERT INTO `product` (price, category, productName, productImg) " + "VALUES(@price, @category, @productName, @productImg)";
                query.Parameters.AddWithValue("@price", product.price);
                query.Parameters.AddWithValue("@category", product.category);
                query.Parameters.AddWithValue("@productName", product.productName);
                query.Parameters.AddWithValue("@productImg", product.productImg);
                int row = query.ExecuteNonQuery();
                if (row != 0)
                {
                    connection.Close();
                    return StatusCode(201, "produkt skapad");
                }
            }catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "produkt skapad");
        }
        [HttpPut]
        public ActionResult UpdateProduct(Product product)
        {
            User user = new User();
            /*string auth = Request.Headers["Authorization"];//GUID
            if (auth == null || LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "du är inte inloggad");
            }

            user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
            if (user.Role != 2)
            {
                return StatusCode(403, "Du har inte rätten till att skapa produkter");
            }*/
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "UPDATE product " + "SET price = @price, category = @category, productName = @productName, productImg = @productImg " +
                    "WHERE productId = @productId";
                query.Parameters.AddWithValue("@price", product.price);
                query.Parameters.AddWithValue("@category", product.category);
                query.Parameters.AddWithValue("@productName", product.productName);
                query.Parameters.AddWithValue("@productImg", product.productImg);
                query.Parameters.AddWithValue("@productId", product.productId);
                int row = query.ExecuteNonQuery();
                if (row != 0)
                {
                    connection.Close();
                    return StatusCode(201, "ändring gick");
                }
            }catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "ändring gick");
        }
    }
}
