using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak1.Models;

namespace Zadatak1.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Delete(string username)
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];
            users.Remove(username);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SearchByRole(string role)
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];
            List<User> usersFilter = new List<User>();

            if(role.Equals(""))
            {
                ViewBag.users = users.Values;
            } 
            else
            {
                foreach (var user in users.Values)
                {
                    if (user.Role.ToString().Equals(role))
                    {
                        usersFilter.Add(user);
                    }
                }
                ViewBag.users = usersFilter;
            }
            
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult SearchByUsername(string username)
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];

            foreach (var user in users.Values)
            {
                if (user.Username.Equals(username))
                {
                    ViewBag.foundUser = user.Username;
                    Console.WriteLine($"Found user: {ViewBag.foundUser}");
                    break;
                }
            }

            if(ViewBag.foundUser == null)
            {
                ViewBag.foundUser = "not_found";
            }

            ViewBag.users = users.Values;

            return View("~/Views/Home/Index.cshtml");
        }
    }
}