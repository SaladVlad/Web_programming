using SOV2WebShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOV2WebShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index","Authenticate");
            }

            List<Product> products = (List<Product>)HttpContext.Application["products"];

            return View(products);
        }
    }
}