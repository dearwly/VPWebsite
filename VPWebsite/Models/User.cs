using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VPWebsite.Models
{
    public class User
    {
        public static User loginedUser;
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public Avatar Avatar { get; set; }

    }
}