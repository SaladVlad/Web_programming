using SOV2_Priprema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOV2_Priprema.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {

            Dictionary<Product, Int32> cart = (Dictionary<Product, Int32>)Session["cart"];
            if (cart == null)
            {
                return RedirectToAction("Index", "Home");
            }

            double totalPrice = 0;
            foreach (var product in cart.Keys)
            {
                totalPrice += (cart[product] * product.price);
            }

            ViewBag.TotalPrice = totalPrice;
            return View(cart);
        }

        [HttpPost]
        public ActionResult Add(string productID)
        {
            Dictionary<Product, Int32> cart = (Dictionary<Product, Int32>)Session["cart"];
            List<Product> products = (List<Product>)HttpContext.Application["products"];
            Product product = products.Find(prod => prod.id.Equals(productID));

            if (cart.ContainsKey(product))
            {
               cart[product]++;
            }
            else
            {
                cart.Add(product, 1);
            }


            return RedirectToAction("Index", "Home");
        }

        public ActionResult Buy()
        {
            Dictionary<Product,int> cart = (Dictionary<Product, Int32>)Session["cart"];
            if(cart == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Session["cart"] = new Dictionary<Product, int>();
            


            return RedirectToAction("Index");
        }
    }
}