using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using MySql.Data.MySqlClient;

namespace VPWebsite.Models
{
    public class User
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public static User loginedUser;
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public Avatar Avatar { get; set; }

        public static string GetUsername(int id)
        {
            if(id == 0)
            {
                return "Anonymous";
            }
            string Username = null;
            string query = "SELECT UserName FROM Users WHERE Id = @UserId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", id);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Username = reader.GetString("Username");
                    }
                }
            }
            return Username;
        }

    }
}