using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System.Collections;
using System.Data;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;


namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("user")]
    [ApiController]
    public class LoginController : ControllerBase{
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");
        public static Hashtable sessionId = new Hashtable();

        [HttpPost("createacc")]
        public IActionResult CreateUser(User user)
        {
            string auth = Request.Headers["Authorization"];
           // user = DecodeUser(user, auth);

            string message = CheckUniqueUserDataExists(user);
            if (message != String.Empty)
            {
                return BadRequest(message);
            }


            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText =
                    "INSERT INTO `user` (`userId`, `userRole`, `username`, `password`, `mail`) " +
                    "VALUES(NULL, 1, @username, @password, @mail)";
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                query.Parameters.AddWithValue("@username", user.Username);
                query.Parameters.AddWithValue("@password", hashedPassword);
                query.Parameters.AddWithValue("@mail", user.Mail);
                int rows = query.ExecuteNonQuery();

                if (rows > 0)
                {
                    Guid guid = Guid.NewGuid();
                    string key = guid.ToString();
                    user.UserId = (int)query.LastInsertedId;
                    sessionId.Add(key, user);
                    connection.Close();
                    return StatusCode(201, key);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginController.CreateUser: " + ex.Message);
                connection.Close();
                return StatusCode(500);
            }
            connection.Close();
            return BadRequest();
        }
        
        [HttpGet("Login")]
        public ActionResult Login()
        {
                string auth = Request.Headers["Authorization"];
                User user = DecodeUser(new User(), auth);
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "";
                //query.Parameters.AddWithValue();
            MySqlDataReader data = query.ExecuteReader();
            try
            {

                string hash = String.Empty;

                while (data.Read())
                {
                    hash = data.GetString("password");
                    user.UserId = data.GetInt32("userId");
                    user.Mail = data.GetString("mail");

                }
                if (hash != String.Empty && BCrypt.Net.BCrypt.Verify(user.Password, hash)) // Crashes when hash is empty
                    {
                        Guid guid = Guid.NewGuid();
                        string key = guid.ToString();
                        sessionId.Add(key, user);
                        connection.Close();
                        return Ok(key);
                    }
                } catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("LoginController.Login: " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return BadRequest("mailadress eller lösenord stämmer inte överens!");
        }
        
        [HttpGet("verify")]
        public ActionResult Verify()
        {
            string auth = Request.Headers["Authorization"];
            if (auth == null
                || !LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "Du behöver vara inloggad!");
            }

            User user = (User)LoginController.sessionId[auth];

            return Ok(user.Role);
        }


        [HttpPost("logout")]
        public ActionResult Logout()
        {
            string auth = Request.Headers["Authorization"];
            if (sessionId.ContainsKey(auth))
            {
                sessionId.Remove(auth);
                return Ok();
            }

            return Unauthorized("Log in to logout");
        }

        private User DecodeUser(User user, string basic)
        {
            basic = basic.Substring(6);
            byte[] bytes = Convert.FromBase64String(basic);
            basic = Encoding.UTF8.GetString(bytes);
            int colon = basic.IndexOf(':');
            string login = basic.Substring(0, colon);
            string password = basic.Substring(colon + 1);

            user.Username = login;
           // user.Login = login;
            user.Password = password;

            return user;
        }
        private string CheckUniqueUserDataExists(User user)
        {
            string message = String.Empty;
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText =
                    "SELECT * FROM user " +
                    "WHERE username = @username " +
                    "WHERE mail = @mail ";
                query.Parameters.AddWithValue("@username", user.Username);
                query.Parameters.AddWithValue("@mail", user.Mail);
                MySqlDataReader data = query.ExecuteReader();

                if (data.Read())
                {
                    if (data.GetString("username") == user.Username)
                    {
                        message = "Användarnamnet finns redan";
                    }
                    if (data.GetString("mail") == user.Mail)
                    {
                        message = "Denna mailadress finns redan";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginController.CheckIfUniqueUserDataExist: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return message;
        }
    }

}
