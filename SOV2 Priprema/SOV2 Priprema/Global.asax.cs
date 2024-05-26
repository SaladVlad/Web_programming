using SOV2_Priprema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SOV2_Priprema
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            HttpContext.Current.Application["products"] 
                = Data.ReadProducts("~/App_Data/products.txt");
            HttpContext.Current.Application["users"]
                = Data.ReadUsers("~/App_Data/users.txt");

        }
    }
}
