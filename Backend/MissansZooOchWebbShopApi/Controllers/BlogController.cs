using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("blog")]
    [ApiController]
    public class BlogController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");

        [HttpPost]
        public ActionResult CreateBlog(Blog blog)
        {
            User user = null;
            string auth = Request.Headers["Authorization"];

            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO `blog` (blogId, userId, title, blogImg, blogText) " +  "VALUES(NULL, (SELECT UserId FROM user WHERE UserId = @userId, @title, @blogImg, @blogText)";
                query.Parameters.AddWithValue("@title", blog.title);
                query.Parameters.AddWithValue("@blogImg", blog.blogImg);
                query.Parameters.AddWithValue("@blogText", blog.blogText);
                query.ExecuteNonQuery();

            }catch(Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "Blog skapad");
        }
    }
}
