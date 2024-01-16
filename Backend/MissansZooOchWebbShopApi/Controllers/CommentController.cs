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
                query.CommandText = "INSERT INTO `comment` (`commentText`, `blogId`, `userId`) VALUES (@commentText, @blogId, @userId);";
                query.Parameters.AddWithValue("@commentText", comment.commentText);
                query.Parameters.AddWithValue("@blogId", comment.blogId);
                query.Parameters.AddWithValue("@userId", user.UserId);
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
        [HttpGet]
        public ActionResult<comment> GetComment(int blogId)
        {
            List<comment> Comment = new List<comment>();
                try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT commentId, commentText, username FROM comment t1 LEFT JOIN user t2 ON t1.userId = t2.userId WHERE blogId = @blogId";
                query.Parameters.AddWithValue("@blogId", blogId);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    comment comment = new comment
                    {
                        commentId = data.GetInt32("commentId"),
                        commentText = data.GetString("commentText"),
                        username = data.GetString("username")
                    };
                    Comment.Add(comment);
                }
            }catch(Exception ex)
            {
                return StatusCode(500);
            }
            return Ok(Comment);
        }
    }
}
