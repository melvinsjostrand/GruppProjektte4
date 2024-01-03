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

        [HttpPost("CreateProduct")] //skapa produkt
        public ActionResult CreateProduct(Product product)
        {
            User user = new User();
          /*  string auth = Request.Headers["Authorization"];//GUID
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

        [HttpDelete("DeleteProduct")] //ta bort produkter
        public ActionResult DeleteBlogAdmin(Product product)
        {
            User user = null;
            /* string auth = Request.Headers["Authorization"];//GUID
             if (auth == null || LoginController.sessionId.ContainsKey(auth))
             {
                 return StatusCode(403, "du är inte inloggad");
             }

             user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
             if (user.Role != 2)
             {
                 return StatusCode(403, "Du har inte rätten till att ta bort bloginlägg");
             }*/
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "DELETE FROM blog WHERE productId = @productId";
                query.Parameters.AddWithValue("@productId", product.productId);
                int row = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "gick inte att ta bort");
            }
            return StatusCode(200, "product har tagits bort");
        }

        [HttpPut("UpdateProduct")] //uppdatera produkter
        public ActionResult UpdateProduct(Product product)
        {
            User user = new User();
           /* string auth = Request.Headers["Authorization"];//GUID
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

        [HttpGet("AllProducts")] //alla produkter
        public ActionResult<List<Product>> GetProduct()
        {
            List<Product> product = new List<Product>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM product";
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Product products = new Product();
                    products.productId = data.GetInt32("productId");
                    products.price = data.GetInt32("price");
                    products.category = data.GetString("category");
                    products.productName = data.GetString("productName");
                    products.productImg = data.GetString("productImg");
                    product.Add(products);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(product);
        }

        [HttpGet("price")] //sortera efter pris
        public ActionResult<Product> GetProduct(int price)
        {
            List<Product> product = new List<Product>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM product ORDER BY price ASC";
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Product products = new Product();
                    products.productId = data.GetInt32("productId");
                    products.price = data.GetInt32("price");
                    products.category = data.GetString("category");
                    products.productName = data.GetString("productName");
                    products.productImg = data.GetString("productImg");
                    product.Add(products);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(product);
        }
        [HttpGet("catergory")] //när du ska söka upp något
        public ActionResult<Product> GetProduct(string category)
        {
            List<Product> product = new List<Product>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM product WHERE category = @category";
                query.Parameters.AddWithValue("@category", category);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Product products = new Product();
                    products.productId = data.GetInt32("productId");
                    products.price = data.GetInt32("price");
                    products.category = data.GetString("category");
                    products.productName = data.GetString("productName");
                    products.productImg = data.GetString("productImg");
                    product.Add(products);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(product);
        }
    }
}
