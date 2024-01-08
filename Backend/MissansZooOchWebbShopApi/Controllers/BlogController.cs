using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("blog")]
    [ApiController]
    public class BlogController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");

        [HttpPost("CreateBlog")] //Skapa blogg
        public ActionResult CreateBlog(Blog blog)
        {
            User user = new User();
            string auth = Request.Headers["Authorization"];//GUID
           /* if (auth == null || LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "du är inte inloggad");
            }

            user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
            if (user.Role != 1)
            {
                return StatusCode(403, "Du har inte rätten till att skapa bloginlägg");
            }*/
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO `blog` (userId, title, blogImg, blogText, username) " + "VALUES(@userId, @title, @blogImg, @blogText, (SELECT username FROM user WHERE UserId = @userId))";
                query.Parameters.AddWithValue("@userId", user.UserId);
                query.Parameters.AddWithValue("@title", blog.title);
                query.Parameters.AddWithValue("@blogImg", blog.blogImg);
                query.Parameters.AddWithValue("@blogText", blog.blogText);
                query.Parameters.AddWithValue("@username", user.Username.ToString());
                int row = query.ExecuteNonQuery();
                if (row != 0)
                {
                    connection.Close();
                    return StatusCode(201, "Blog skapad");
                }

            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "Blog skapad");
        }

        [HttpDelete("DeleteBlogAdmin")] //ta bort blogg som admin
        public ActionResult DeleteBlogAdmin(Blog blog)
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
                 return StatusCode(403, "Du har inte rätten till att ta bort blogginlägg");
             }*/
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
            /* string auth = Request.Headers["Authorization"];//GUID
             if (auth == null || LoginController.sessionId.ContainsKey(auth))
             {
                 return StatusCode(403, "du är inte inloggad");
             }

             user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
             if (user.Role != 1)
             {
                 return StatusCode(403, "Du har inte rätten till att ta bort blogginlägg");
             }*/
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
        public ActionResult<List<Blog>> GetBlog()
        {
            List<Blog> blog = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog";
                MySqlDataReader data = query.ExecuteReader();
                
                while (data.Read()) 
                {
                    Blog blogs = new Blog();
                    blogs.blogId = data.GetInt32("blogId");
                    blogs.title = data.GetString("title");
                    blogs.blogImg = data.GetString("blogImg");
                    blogs.blogText = data.GetString("blogtext");
                    blogs.username = data.GetString("username");
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
        public ActionResult<Blog> GetBlog(int blogId)
        {
            List<Blog> blog = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog WHERE blogId = @blogId";
                query.Parameters.AddWithValue("@blogId", blogId);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Blog blogs = new Blog();
                    blogs.blogId = data.GetInt32("blogId");
                    blogs.title = data.GetString("title");
                    blogs.blogImg = data.GetString("blogImg");
                    blogs.blogText = data.GetString("blogtext");
                    blogs.username = data.GetString("username");
                    blog.Add(blogs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }
            return Ok(blog);
        }
        [HttpGet("user/{username}")]
        public ActionResult<Blog> GetBlog(string username)
        {
            List<Blog> blog = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM blog WHERE username = @username";
                query.Parameters.AddWithValue("@username", username);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Blog blogs = new Blog();
                    blogs.blogId = data.GetInt32("blogId");
                    blogs.title = data.GetString("title");
                    blogs.blogImg = data.GetString("blogImg");
                    blogs.blogText = data.GetString("blogtext");
                    blogs.username = data.GetString("username");
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
