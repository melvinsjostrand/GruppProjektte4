using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("Blog")]
    [ApiController]
    public class BlogController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");
        private Comment comments;

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
                blog.Img = SaveImage(blog.Img);
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO `blog` (title, Img, Text, time, userId) " + "VALUES(@title, @Img, @Text, (SELECT CURRENT_TIMESTAMP),@userId)";
                query.Parameters.AddWithValue("@title", blog.title);
                query.Parameters.AddWithValue("@Img", blog.Img);
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
            string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + GenerateRandomString(8);
            string path = "../../FrontEnd/images/" + uniqueFileName + "." + fileType;
            System.IO.File.WriteAllBytes(path, imageData);

            return path;
        }


        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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
                        Img = data.GetString("Img"),
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
            Blog blog = new Blog();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog t1 LEFT JOIN user t2 ON t1.userId = t2.Id WHERE t1.Id = @Id";
                query.Parameters.AddWithValue("@Id", Id);
                using (MySqlDataReader data = query.ExecuteReader())
                {
                    if (data.Read())
                    {
                        blog = new Blog
                        {
                            Id = data.GetInt32("Id"),
                            title = data.GetString("title"),
                            Img = data.GetString("Img"),
                            text = data.GetString("Text"),
                            time = data.GetString("time"),
                            username = data.GetString("username"),
                        };
                    }
                } 

                query.CommandText = "SELECT t1.Id, text, username, blogId FROM comment t1 LEFT JOIN user t2 ON t1.userId = t2.id WHERE blogId = @Id";
                query.Parameters.Clear(); 
                query.Parameters.AddWithValue("@Id", Id);
                using (MySqlDataReader dataComment = query.ExecuteReader())
                {
                    while (dataComment.Read())
                    {
                        Comment comments = new Comment
                        {
                            Id = dataComment.GetInt32("Id"),
                            text = dataComment.GetString("text"),
                            blogId = dataComment.GetInt32("blogId"),
                            username = dataComment.GetString("username")
                        };
                        blog.AddComment(comments);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            finally
            {
                // Make sure to close the connection in a finally block
                connection.Close();
            }
            return blog;
        }
            [HttpGet("User")]
            public ActionResult<Blog> GetAllBlogFromUserId()
            {
            string auth = Request.Headers["Authorization"];//GUID
            if (auth == null || !LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "du är inte inloggad");
            }

            User user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
            List<Blog> blog = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog t1 LEFT JOIN user t2 ON t1.userId = t2.Id WHERE userId = @userId ORDER BY time ASC";
                query.Parameters.AddWithValue("@userId", user.Id);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Blog blogs = new Blog
                    {
                        Id = data.GetInt32("Id"),
                        title = data.GetString("title"),
                        Img = data.GetString("Img"),
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
