using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Reflection.Metadata;

namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("Comment")]
    [ApiController]
    public class CommentController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");
        public CommentController()
        {
            Console.WriteLine("Comment");
        }
        [HttpPost]
        public ActionResult PostComment(Comment comment) 
        {
            //Comment comment = new Comment();
            string auth = Request.Headers["Authorization"];//GUID
              if (auth == null || !LoginController.sessionId.ContainsKey(auth))
             {
                 return StatusCode(403, "du är inte inloggad");
             }

             User user = (User)LoginController.sessionId[auth]; //id Role username hashedpassword mail
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO `comment` (`text`, `blogId`, `userId`) VALUES (@commentText, @blogId, @Id);";
                query.Parameters.AddWithValue("@commentText", comment.text);
                query.Parameters.AddWithValue("@blogId", comment.blogId);
                query.Parameters.AddWithValue("@Id", user.Id);
                int row = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("Skapades ej " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(201, "kommentar skapad");
        }

        [HttpGet]
        public ActionResult<Comment> GetComment(int blogId)
        {
            List<Comment> comment = new List<Comment>();
                try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT t1.Id, text, username, blogId FROM comment t1 LEFT JOIN user t2 ON t1.userId = t2.id WHERE blogId = @blogId";
                query.Parameters.AddWithValue("@blogId", blogId);
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Comment comments = new Comment
                    {
                        Id = data.GetInt32("Id"),
                        text = data.GetString("text"),
                        blogId = data.GetInt32("blogId"),
                        username = data.GetString("username")

                    };
                    comment.Add(comments);
                }
            }catch(Exception ex)
            {
                return StatusCode(500);
            }
            return Ok(comment);
        }
    }
}
