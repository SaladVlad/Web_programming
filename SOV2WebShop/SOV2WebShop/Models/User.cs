using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOV2WebShop.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}