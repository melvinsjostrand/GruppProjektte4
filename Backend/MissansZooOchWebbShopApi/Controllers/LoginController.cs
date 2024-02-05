using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;


namespace MissansZooOchWebbShopApi.Controllers
{
    [Route("User")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=webbshop");
        public static Hashtable sessionId = new Hashtable();

        [HttpPost]
        public IActionResult CreateUser(User user)
        
        
        
        {
            string auth = Request.Headers["Authorization"];
            Console.WriteLine(user.mail + " "+ user.username);
            string message = CheckIfUniqueUser(user);
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
                    "INSERT INTO `user` (`Role`, `username`, `password`, `mail`, address) " +
                    "VALUES(1, @username, @password, @mail, @address)";
                string hashedpassword = BCrypt.Net.BCrypt.HashPassword(user.password);
                query.Parameters.AddWithValue("@username", user.username);
                query.Parameters.AddWithValue("@password", hashedpassword);
                query.Parameters.AddWithValue("@mail", user.mail);
                query.Parameters.AddWithValue("@address", user.address);
                int rows = query.ExecuteNonQuery();

                if (rows > 0)
                {
                    Guid guid = Guid.NewGuid();
                    string key = guid.ToString();
                    user.Id = (int)query.LastInsertedId;
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
            query.CommandText = "SELECT * FROM user WHERE mail = @mail";
            query.Parameters.AddWithValue("@mail", user.mail);
            MySqlDataReader data = query.ExecuteReader();
            try
            {

                string hash = String.Empty;

                while (data.Read())
                {
                    hash = data.GetString("password");
                    user.Id = data.GetInt32("Id");
                    user.mail = data.GetString("mail");
                    user.Role = data.GetInt32("Role");
                }
                if (hash != String.Empty && BCrypt.Net.BCrypt.Verify(user.password, hash)) // Crashes when hash is empty
                {
                    Guid guid = Guid.NewGuid();
                    string key = guid.ToString();
                    sessionId.Add(key, user);
                    connection.Close();
                    Console.WriteLine(key, user);
                    return Ok(key);
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("LoginController.Login: " + ex.Message);
                return StatusCode(500);
            }
            connection.Close();
            return BadRequest("mailadress eller lösenord stämmer inte överens!");
        }

        [HttpGet("Verify")]
        public ActionResult Verify()
        {
            string auth = Request.Headers["Authorization"];
            if (auth == null
                || !LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(404);
            }

            User user = (User)LoginController.sessionId[auth];

            return Ok(user.Role);
        }
        [HttpGet("password")]
        public ActionResult password()
        {
            string auth = Request.Headers["Authorization"];
            if (auth == null || !LoginController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "du är inte inloggad");
            }

            User user = (User)LoginController.sessionId[auth]; //Id Role username hashedpassword mail
            if (user.Role != 2)
            {
                return StatusCode(403, "Du har inte rätten till att skapa produkter");
            }
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT password FROM user WHERE Id = Id";
                query.Parameters.AddWithValue("@Id", user.Id);
                query.ExecuteNonQuery();
            }catch (Exception ex)
            {
                connection.Close();
                return StatusCode(500);
            }
            return StatusCode(200);
        }

        [HttpPost("Logout")]
        public ActionResult Logout()
        {
            string auth = Request.Headers["Authorization"];
            if (sessionId.ContainsKey(auth))
            {
                sessionId.Remove(auth);
                return StatusCode(200, "you have signed out");
            }

            return Unauthorized("Log in to logout");
        }
        [HttpPut("ChangeRole")]
        public ActionResult UpdateRole() //funkar ej
        {
                string auth = Request.Headers["Authorization"];
                if (auth == null || !LoginController.sessionId.ContainsKey(auth))
                    {
                        return StatusCode(403, "du är inte inloggad");
                    }

                User user = (User)LoginController.sessionId[auth]; //Id Role username hashedpassword mail
                if (user.Role != 2)
                {
                    return StatusCode(403, "Du har inte rätten till att skapa produkter");
                }
                try
                {
                    connection.Open();
                    MySqlCommand query = connection.CreateCommand();
                    query.Prepare();
                    query.CommandText = "UPDATE `user` " + "SET `Role` = @Role " + "WHERE `username` = @username";
                    query.Parameters.AddWithValue("@Role", user.Role);
                    query.Parameters.AddWithValue("@username", user.username);
                    int row = query.ExecuteNonQuery();
                }catch (Exception ex)
                {
                    connection.Close();
                    return StatusCode(500);
                }
                connection.Close();
                return StatusCode(200, "roll ändrar");
            }
        [HttpPut("Changepassword")]
        public ActionResult Changepassword(User user) //funkar ej
        {
            try

            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "UPDATE `user` " + "SET `password` = @password " + "WHERE `Id` = @Id";
                string hashedpassword = BCrypt.Net.BCrypt.HashPassword(user.password);
                query.Parameters.AddWithValue("@password", hashedpassword);
                query.Parameters.AddWithValue("@Id", user.Id);
                int row = query.ExecuteNonQuery();
            }catch (Exception ex)
            {
                connection.Close();
                return StatusCode(500);
            }
            connection.Close();
            return StatusCode(200, "lösenord ändrar");
        }
        private User DecodeUser(User user, string auth)
        {
            if (auth != null && auth.StartsWith("Basic"))
            {
                string encodedusernamepassword = auth.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                string usernamepassword = encoding.GetString(Convert.FromBase64String(encodedusernamepassword));
                int seperatorIndex = usernamepassword.IndexOf(':');
                user.mail = usernamepassword.Substring(0, seperatorIndex);
                user.password = usernamepassword.Substring(seperatorIndex + 1);
            }
            else
            {
                //Handle what happens if that isn't the case
                throw new Exception("The authorization header is either empty or isn't Basic.");
            }
            return user;
        }
        private string CheckIfUniqueUser(User user)
        {
            string message = String.Empty;
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM user WHERE username = @username OR mail = @mail";
                query.Parameters.AddWithValue("@username", user.username);
                query.Parameters.AddWithValue("@mail", user.mail);
                MySqlDataReader data = query.ExecuteReader();


                if (data.Read())
                {
                    if (data.GetString("username") == user.username)
                    {
                        message = "Användarnamnet finns redan";
                    }
                    if(data.GetString("mail") == user.mail)
                    {
                        message = "Denna mailadress finns redan";
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return message;
        }
        [HttpDelete]
        public ActionResult Delete(User user)
        {

            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "DELETE FROM user WHERE Id = @Id";
                query.Parameters.AddWithValue("@Id", user.Id);

                MySqlDataReader data = query.ExecuteReader();
            }catch(Exception ex)
            {
                return StatusCode(500, "FEL");
            }
            return StatusCode(200, "användare borttagen");
        }
        [HttpGet("AllUsers")]
        public ActionResult<User> GetUser()
        {
            List<User> user = new List<User>();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT Id , username FROM user";
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    User users = new User
                    {
                        Id = data.GetInt32("Id"),
                        username = data.GetString("username")
                    };
                    user.Add(users);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
            return Ok(user);
        }
    }
}
    
