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

        [HttpPost("CreateBlog")]
        public ActionResult CreateBlog(Blog blog)
        {
            User user = null;
            string auth = Request.Headers["Authorization"];//GUID
            if(auth == null || LoginController.sessionId.ContainsKey(auth)) 
            {
                return StatusCode(403, "du är inte inloggad");
            }

            user = (User)LoginController.sessionId[auth]; //userId Role username hashedpassword mail
            if (user.Role != 1)
            {
                return StatusCode(403, "Du har inte rätten till att skapa bloginlägg");
            }
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO `blog` (userId, title, blogImg, blogText, username) " +  "VALUES(@userId, @title, @blogImg, @blogText, (SELECT username FROM user WHERE UserId = @userId))";
                query.Parameters.AddWithValue("@userId", user.UserId);
                query.Parameters.AddWithValue("@title", blog.title);
                query.Parameters.AddWithValue("@blogImg", blog.blogImg);
                query.Parameters.AddWithValue("@blogText", blog.blogText);
                query.Parameters.AddWithValue("@username", user.Username.ToString());
                int row = query.ExecuteNonQuery();
                if(row != 0) 
                {
                    connection.Close();
                    return StatusCode(201, "Blog skapad");
                }
                
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
