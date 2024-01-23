using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("Blog")]
    [ApiController]
    public class BlogController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");

        [HttpPost] //Skapa blogg
        public ActionResult CreateBlog(Blog blog)
        {
            Console.WriteLine(blog + "hej hej hej");
            string auth = Request.Headers["Authorization"];//GUID
             if (auth == null || !LoginController.sessionId.ContainsKey(auth))
             {
                 return StatusCode(403, "du är inte inloggad");
             }

             User user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
            if (user.Role != 1)
             {
                 return StatusCode(403, "Du har inte rätten till att skapa blogginlägg");
             }
            try
            {
                Console.WriteLine(blog.img + "hej hej hej");
                blog.img = SaveImage(blog.img);
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO `blog` (title, Img, Text, time, userId) " + "VALUES(@title, @Img, @Text, (SELECT CURRENT_TIMESTAMP),@userId)";
                query.Parameters.AddWithValue("@title", blog.title);
                query.Parameters.AddWithValue("@Img", blog.img);
                query.Parameters.AddWithValue("@Text", blog.text);
                query.Parameters.AddWithValue("@time", blog.time);
                query.Parameters.AddWithValue("@userId", user.Id);
                int row = query.ExecuteNonQuery();
            }catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "Blog skapad");
        }

        private string SaveImage(string base64)
        {
            string fileType = base64.Split(",")[0].Split("/")[1].Split(";")[0];
            byte[] imageData = Convert.FromBase64String(base64.Split(",")[1]);

            string path = "../../FrontEnd/images/bild." + fileType;
            System.IO.File.WriteAllBytes(path, imageData);

            return path;
        }

        [HttpDelete("DeleteBlogAdmin")] //ta bort blogg som admin
        public ActionResult DeleteBlogAdmin(Blog blog)
        {
             User user = null;
             string auth = Request.Headers["Authorization"];//GUID
             if (auth == null || !LoginController.sessionId.ContainsKey(auth))
             {
                 return StatusCode(403, "du är inte inloggad");
             }

             user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
             if (user.Role != 2)
             {
                 return StatusCode(403, "Du har inte rätten till att ta bort blogginlägg");
             }
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "DELETE FROM blog WHERE Id = @Id";
                query.Parameters.AddWithValue("@Id", blog.Id);
                int row = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "gick inte att ta bort");
            }
            return StatusCode(200, "blogg har tagits bort");
        }
        [HttpDelete("DeleteBlogUser")] //ta bort egetBloginlägg
        public ActionResult DeleteBlogUser(Blog blog)
        {
             User user = null;
             string auth = Request.Headers["Authorization"];//GUID
             if (auth == null || !LoginController.sessionId.ContainsKey(auth))
             {
                 return StatusCode(403, "du är inte inloggad");
             }

             user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
             if (user.Role != 1)
             {
                 return StatusCode(403, "Du har inte rätten till att ta bort blogginlägg");
             }
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "DELETE FROM blog WHERE Id = @Id";
                query.Parameters.AddWithValue("@Id", blog.Id);
                int row = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "gick inte att ta bort");
            }
            return StatusCode(200, "blogg har tagits bort");
        }

        [HttpGet("AllBlog")]
        public ActionResult<List<Blog>> GetAllBlogs()
        {
            List<Blog> blog = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog t1 LEFT JOIN user t2 ON t1.userId = t2.Id ORDER BY time DESC";
                MySqlDataReader data = query.ExecuteReader();
                
                while (data.Read()) 
                {
                    Blog blogs = new Blog
                    {
                        Id = data.GetInt32("Id"),
                        title = data.GetString("title"),
                        img = data.GetString("Img"),
                        text = data.GetString("Text"),
                        time = data.GetString("time"),
                        username = data.GetString("username")
                    };
                    blog.Add(blogs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(blog);
        }
        [HttpGet("{Id}")]
        public ActionResult<Blog> GetBlogFromId(int Id)
        {
            List<Blog> blog = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog t1 LEFT JOIN user t2 ON t1.userId = t2.userId WHERE Id = @Id ORDER BY time ASC";
                query.Parameters.AddWithValue("@Id", Id);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Blog blogs = new Blog
                    {
                        Id = data.GetInt32("Id"),
                        title = data.GetString("title"),
                        img = data.GetString("Img"),
                        text = data.GetString("Text"),
                        time = data.GetString("time"),
                        username = data.GetString("username")
                    };
                    blog.Add(blogs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(blog);
        }
        [HttpGet("User/{userId}")]
        public ActionResult<Blog> GetAllBlogFromUserId(int userId)
        {
            List<Blog> blog = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog t1 LEFT JOIN user t2 ON t1.userId = t2.Id WHERE userId = @userId ORDER BY time ASC";
                query.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Blog blogs = new Blog
                    {
                        Id = data.GetInt32("Id"),
                        title = data.GetString("title"),
                        img = data.GetString("Img"),
                        text = data.GetString("Text"),
                        time = data.GetString("time"),
                        username = data.GetString("username")
                    };
                    blog.Add(blogs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(blog);
        }
    }
}
