using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("Product")]
    [ApiController]
    public class ProductController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");

        [HttpPost] //skapa produkt
        public ActionResult CreateProduct(Product product)
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
                query.CommandText = "INSERT INTO `product` (price, category, productName, productImg, description, stock, content, feeding) " + "VALUES(@price, @category, @productName, @productImg, @description, @stock, @content, @feeding)";
                query.Parameters.AddWithValue("@price", product.price);
                query.Parameters.AddWithValue("@category", product.category);
                query.Parameters.AddWithValue("@productName", product.productName);
                query.Parameters.AddWithValue("@productImg", product.productImg);
                query.Parameters.AddWithValue("@description", product.description);
                query.Parameters.AddWithValue("@stock", product.stock);
                query.Parameters.AddWithValue("@content", product.content);
                query.Parameters.AddWithValue("@feeding", product.feeding);
                int row = query.ExecuteNonQuery();
            }catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "produkt skapad");
        }

        private string SaveImage(object item)
        {
            string imgData;

            if (item is Blog blog)
            {
                imgData = blog.blogImg;
            }
            else if (item is Product product)
            {
                imgData = product.productImg;
            }
            else
            {
                throw new ArgumentException("Invalid object type. Supported types are Blog and Product.");
            }

            string fileType = imgData.Split(",")[0].Split("/")[1].Split(";")[0];
            byte[] imageData = Convert.FromBase64String(imgData.Split(",")[1]);

            string path = "../../FrontEnd/images/bild." + fileType;
            System.IO.File.WriteAllBytes(path, imageData);

            return path;
        }
        [HttpDelete] //ta bort produkter
        public ActionResult DeleteBlogAdmin(Product product)
        {
            User user = null;
             string auth = Request.Headers["Authorization"];//GUID
             if (auth == null || LoginController.sessionId.ContainsKey(auth))
             {
                 return StatusCode(403, "du är inte inloggad");
             }

             user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
             if (user.Role != 2)
             {
                 return StatusCode(403, "Du har inte rätten till att ta bort bloginlägg");
             }
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "DELETE FROM product WHERE productId = @productId";
                int row = query.ExecuteNonQuery();
            }catch (Exception ex)
            {
                return StatusCode(500, "gick inte att ta bort");
            }
            return StatusCode(200, "product har tagits bort");
        }
        [HttpPut("ChangeRating")]
        public ActionResult ChangeRating(Product product) 
        {
            User user = new User();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "UPDATE product " + "SET rating = @rating " + "WHERE productId = @productId";
                query.Parameters.AddWithValue("@rating", product.rating);
                int row = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "rating ändrad");
        }
        [HttpPut("UpdateProduct")] //uppdatera produkter
        public ActionResult UpdateProduct(Product product)
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
                query.CommandText = "UPDATE product " + "SET price = @price, category = @category, productName = @productName, productImg = @productImg " +
                    "WHERE productId = @productId";
                query.Parameters.AddWithValue("@price", product.price);
                query.Parameters.AddWithValue("@category", product.category);
                query.Parameters.AddWithValue("@productName", product.productName);
                query.Parameters.AddWithValue("@productImg", product.productImg);
                int row = query.ExecuteNonQuery();
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
        public ActionResult<List<Product>> GetAllProducts()
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
                    Product products = new Product
                    {
                        price = data.GetInt32("price"),
                        category = data.GetString("category"),
                        productName = data.GetString("productName"),
                        productImg = data.GetString("productImg"),
                        productId = data.GetInt32("productId"),
                        content = data.GetString("content"),
                        feeding = data.GetString("feeding")
                    };
                    product.Add(products);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(product);
        }

        [HttpGet("Price")] //sortera efter pris
        public ActionResult<Product> GetProductSortedByPrice()
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
                    Product products = new Product
                    {
                        price = data.GetInt32("price"),
                        category = data.GetString("category"),
                        productName = data.GetString("productName"),
                        productImg = data.GetString("productImg"),
                        productId = data.GetInt32("productId"),
                        content = data.GetString("content"),
                        feeding = data.GetString("feeding")
                    };
                    product.Add(products);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(product);
        }
        [HttpGet("ProductName")] //sortera efter pris
        public ActionResult<Product> GetProductSortedByName()
        {
            List<Product> product = new List<Product>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM product ORDER BY productName ASC";
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Product products = new Product
                    {
                        price = data.GetInt32("price"),
                        category = data.GetString("category"),
                        productName = data.GetString("productName"),
                        productImg = data.GetString("productImg"),
                        productId = data.GetInt32("productId"),
                        content = data.GetString("content"),
                        feeding = data.GetString("feeding")
                    };
                    product.Add(products);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(product);
        }
        [HttpGet("Category/{category}")] //när du ska söka upp något
        public ActionResult<Product> GetProductByCategory(string category)
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
                    Product products = new Product
                    {
                        price = data.GetInt32("price"),
                        category = data.GetString("category"),
                        productName = data.GetString("productName"),
                        productImg = data.GetString("productImg"),
                        productId = data.GetInt32("productId"),
                        content = data.GetString("content"),
                        feeding = data.GetString("feeding")
                    };
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
