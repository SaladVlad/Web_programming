using SOV2WebShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SOV2WebShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            HttpContext.Current.Application["users"] = new List<User>() {
                new User("pera","pera"),
                new User("admin","admin")
            };

            HttpContext.Current.Application["products"] = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name="Phone",
                    Price=20000
                },
                new Product()
                {
                    Id = 2,
                    Name="Laptop",
                    Price=50000
                }
            };
        }
    }
}
