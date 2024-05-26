using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebShopMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Učitamo proizvode iz datoteke u memoriju
            Products products = new Products("~/App_Data/products.txt");
            HttpContext.Current.Application["Products"] = products;
        }
    }
}
