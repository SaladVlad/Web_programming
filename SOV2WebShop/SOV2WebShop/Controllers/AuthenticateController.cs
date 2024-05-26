using SOV2WebShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOV2WebShop.Controllers
{
    public class AuthenticateController : Controller
    {
        // GET: Authenticate
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username,string password)
        {

            List<User> users = (List<User>)(HttpContext.Application["users"]);

            if (users.Find(u => u.Username.Equals(username) && u.Password.Equals(password))==null)
            {
                ViewBag.Message = "Invalid username or password";
                return View("Index");
            }

            Session["user"] = username;
            Session["cart"] = new Dictionary<Product, int>();

            return RedirectToAction("Index","Home");
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            List<User> users = (List<User>)(HttpContext.Application["users"]);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Message = "Invalid data entered!";
                return RedirectToAction("Index","Authenticate");
            }
            if (users.Find(u => u.Username.Equals(username)) != null)
            {
                ViewBag.Message = "User already exists!";
                return RedirectToAction("Index", "Authenticate");
            }

            User user = new User(username, password);
            users.Add(user);
            Session["user"] = username;
            Session["cart"] = new Dictionary<Product,int>();

            return RedirectToAction("Index","Home");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["cart"] = null;
            return View("Index");
        }
    }
}