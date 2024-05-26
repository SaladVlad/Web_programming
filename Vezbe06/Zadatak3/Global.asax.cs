﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Zadatak3.Models;

namespace Zadatak3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            string path = Path.Combine(Server.MapPath("~/Files/"));
            List<UploadedFile> files = new List<UploadedFile>();
            foreach (string file in Directory.GetFiles(path))
            {
                files.Add(new UploadedFile(Path.GetFileName(file), file));
            }
            HttpContext.Current.Application["Files"] = files;
        }
    }
}
