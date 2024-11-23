using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VPWebsite.Models;

namespace VPWebsite.Controllers
{
    public class AccountController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = GetUserByCredentials(username, password);
            if (user != null)
            {
                Session["User"] = user.Username;
                Session["IsAdmin"] = (user.Role == Models.UserRole.Admin) ? 1 : 0;
                Models.User.loginedUser = user;
                updateCurrentAvatar();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Invalid login credentials.";
            return View();
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                AddUser(model);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // GET: Logout
        public ActionResult Logout()
        {
            Session["User"] = null;
            Session["IsAdmin"] = null;
            Models.User.loginedUser = null;
            updateCurrentAvatar();
            return RedirectToAction("Index", "Home");
        }

        // 方法：通过用户名和密码从数据库中获取用户
        private User GetUserByCredentials(string username, string password)
        {
            User user = null;
            string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Password = reader.GetString("Password"),
                            Role = (reader.GetInt16("IsAdmin") == 1) ? Models.UserRole.Admin : Models.UserRole.User,
                            Avatar = new Avatar { AvatarName = !reader.IsDBNull(reader.GetOrdinal("avatars")) ? reader.GetString("avatars") : null, AvatarPath = null}
                        };
                    }
                }
            }

            return user;
        }

        // 方法：添加新用户到数据库
        private void AddUser(User user)
        {
            string query = "INSERT INTO Users (Username, Password, IsAdmin) VALUES (@Username, @Password, @IsAdmin)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@IsAdmin", 0);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void updateCurrentAvatar()
        {
            if (Models.User.loginedUser != null)
            {
                Session["Avatar"] = Models.User.loginedUser.Avatar.AvatarName;
            }
            else
            {
                Session["Avatar"] = null;
            }
        }
    }
}
