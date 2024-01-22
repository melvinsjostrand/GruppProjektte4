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
                blog.blogImg = img(blog);
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO `blog` (title, blogImg, blogText, time, userId) " + "VALUES(@title, @blogImg, @blogText, (SELECT CURRENT_TIMESTAMP),@userId)";
                query.Parameters.AddWithValue("@title", blog.title);
                query.Parameters.AddWithValue("@blogImg", blog.blogImg);
                query.Parameters.AddWithValue("@blogText", blog.blogText);
                query.Parameters.AddWithValue("@time", blog.time);
                query.Parameters.AddWithValue("@userId", user.UserId);
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
        private string img(Blog blog)
        {
            string fileType = blog.blogImg.Split(",")[0].Split("/")[1].Split(";")[0];
            byte[] imageData = Convert.FromBase64String(blog.blogImg.Split(",")[1]);
            string path = ("images/") + "bild" +(".")+ fileType;
            System.IO.File.WriteAllBytes(path,imageData);
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
                query.CommandText = "DELETE FROM blog WHERE blogId = @blogId";
                query.Parameters.AddWithValue("@blogId", blog.blogId);
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
                query.CommandText = "DELETE FROM blog WHERE blogId = @blogId";
                query.Parameters.AddWithValue("@blogId", blog.blogId);
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
                query.CommandText = "SELECT * FROM blog t1 LEFT JOIN user t2 ON t1.userId = t2.userId ORDER BY time DESC";
                MySqlDataReader data = query.ExecuteReader();
                
                while (data.Read()) 
                {
                    Blog blogs = new Blog
                    {
                        blogId = data.GetInt32("blogId"),
                        title = data.GetString("title"),
                        blogImg = data.GetString("blogImg"),
                        blogText = data.GetString("blogtext"),
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
        [HttpGet("{blogId}")]
        public ActionResult<Blog> GetBlogFromBlogId(int blogId)
        {
            List<Blog> blog = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog t1 LEFT JOIN user t2 ON t1.userId = t2.userId WHERE blogId = @blogId ORDER BY time ASC";
                query.Parameters.AddWithValue("@blogId", blogId);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Blog blogs = new Blog
                    {
                        blogId = data.GetInt32("blogId"),
                        title = data.GetString("title"),
                        blogImg = data.GetString("blogImg"),
                        blogText = data.GetString("blogtext"),
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
                query.CommandText = "SELECT * FROM blog t1 LEFT JOIN user t2 ON t1.userId = t2.userId WHERE userId = @userId ORDER BY time ASC";
                query.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Blog blogs = new Blog
                    {
                        blogId = data.GetInt32("blogId"),
                        title = data.GetString("title"),
                        blogImg = data.GetString("blogImg"),
                        blogText = data.GetString("blogtext"),
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
