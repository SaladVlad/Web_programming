using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

public class Products
{
    public Dictionary<string, Product> list { get; set; }

    public Products(string path)
    {
        path = HostingEnvironment.MapPath(path);
        list = new Dictionary<string, Product>();
        FileStream stream = new FileStream(path, FileMode.Open);
        StreamReader sr = new StreamReader(stream);
        string line = "";
        while ((line = sr.ReadLine()) != null)
        {
            string[] tokens = line.Split(';');
            Product p = new Product(tokens[0], tokens[1], double.Parse(tokens[2]));
            list.Add(p.id, p);
        }
        sr.Close();
        stream.Close();
    }
}
