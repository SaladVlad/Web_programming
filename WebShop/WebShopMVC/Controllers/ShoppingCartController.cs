using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart sc = (ShoppingCart)Session["sc"];
            if (sc == null)
            {
                sc = new ShoppingCart();
                Session["sc"] = sc;
            }
            ViewBag.sc = sc;
            // Pređi na ShoppingCart/Index.cshtml View
            return View();
        }

        [HttpPost]
        // POST: ShoppingCart/Add
        public ActionResult Add(ShoppingCartFormParams item)
        {
            Products products = (Products)HttpContext.Application["products"];
            ShoppingCart sc = (ShoppingCart)Session["sc"];
            if (sc == null)
            {
                sc = new ShoppingCart();
                Session["sc"] = sc;
            }

            if (!item.id.Equals("") && !item.count.Equals(""))
            {
                sc.Add(new ShoppingCartItem(products.list[item.id], item.count));
            }
            ViewBag.sc = sc;
            // Pređi na ShoppingCart/Index.cshtml View
            return View("Index"); // Da smo vratili View(), otišao bi na: ShoppingCart/Add.cshtml, a to nemamo.
        }
    }
}