using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Reflection.Metadata;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("Comment")]
    [ApiController]
    public class Commentcontroller : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");
        [HttpPost]
        public ActionResult PostComment(comment comment) 
        {
            User user = new User();
            Blog blog = new Blog();
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
                query.CommandText = "INSERT INTO `comment` (blogId, userId, comment) " + "VALUES(@blogId, @userId, @comment)";
                query.Parameters.AddWithValue("@blogId", comment.blogId);
                query.Parameters.AddWithValue("@userId", user.UserId);
                query.Parameters.AddWithValue("@comment", comment.commentText);
                int row = query.ExecuteNonQuery();
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
    }
}
