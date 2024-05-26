using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak2.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType Role { get; set; }

        public int Age { get; set; }

        public bool LoggedIn { get; set; }

        public User()
        {
            Username = "";
            Password = "";
            LoggedIn = false;
        }

        public User(string username, string password, UserType type, int age)
        {
            Username = username;
            Password = password;
            Role = type;
            LoggedIn = false;
            Age = age;
        }
        public override bool Equals(object obj)
        {
            return ((User)obj).Username.Equals(this.Username);
        }
    }
}