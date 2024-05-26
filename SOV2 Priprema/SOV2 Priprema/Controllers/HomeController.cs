using SOV2_Priprema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOV2_Priprema.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            User user = (User)Session["user"];
            if (user == null || user.Username.Equals(""))
            {
                return RedirectToAction("Index","Authentication");
            }
            Dictionary<Product, int> cart = (Dictionary<Product, int>)Session["cart"];

            if(cart == null)
            {
                cart = new Dictionary<Product, int>();
                Session["cart"] = cart;
            }

            ViewBag.User = user;

            List<Product> products = (List<Product>)HttpContext.Application["products"];

            return View(products);
        }
    }
}