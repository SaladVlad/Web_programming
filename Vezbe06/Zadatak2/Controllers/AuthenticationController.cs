using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak2.Models;

namespace Zadatak2.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            User user = new User();
            Session["user"] = user;
            return View(user);
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            // Model validations next week 
            List<User> users = (List<User>)HttpContext.Application["Users"];
            if(users.Contains(user))
            {
                ViewBag.Message = $"User with {user.Username} already exists!";
                return View();
            }

            users.Add(user);
            Data.SaveUser(user);
            Session["user"] = user;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            List<User> users = (List<User>)HttpContext.Application["Users"];
            User user = users.Find(u => u.Username.Equals(username) && u.Password.Equals(password));

            if(user == null)
            {
                ViewBag.Message = $"User with credentials does not exist!";
                return View("Index");
            }

            Session["user"] = user;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["cart"] = null;

            return View("Index");

        }
    }
}