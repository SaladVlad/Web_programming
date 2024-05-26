using SOV2WebShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOV2WebShop.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            Dictionary<Product,int> products = (Dictionary<Product, int>)Session["cart"];

            return View(products);
        }

        [HttpPost]
        public ActionResult Add(int productId)
        {
            Dictionary<Product, int> products = (Dictionary<Product,int>)Session["cart"];
            List<Product> productsList = (List<Product>)HttpContext.Application["products"];

            foreach (Product product in productsList)
            {
                if(product.Id == productId)
                {
                    if(!products.ContainsKey(product)) products[product] = 1;
                    else products[product] += 1;
                }
            }

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult Buy()
        {
            Dictionary<Product, int> products = (Dictionary<Product, int>)Session["cart"];
            products.Clear();
            return View("Index",products);
        }
    }
}