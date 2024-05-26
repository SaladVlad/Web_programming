using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace WebShopMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Products products = (Products)HttpContext.Application["products"];
            //Products products = HttpContext.Cache["products"];

            ViewBag.products = products;

            return View();
        }

        [HttpGet]
        public ActionResult LoadSadrzaj(String nazivKategorije)
        {
            Products products = (Products) HttpContext.Application["products"];
            Product p = null;
            foreach (String key in products.list.Keys)
            {
                if (products.list[key].name.Equals(nazivKategorije))
                {
                    p = products.list[key];
                    break;
                }
            }

            if (p == null)
            {
                return Content(@"
                        <html>
                        <body>
                            <div id='produkt'> Nema produkta sa imenom: " + nazivKategorije + @"</div>
                        </body>
                        </html>", "text/html");
            }

            return View(p);
        }

    }
}