﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak2.Models;

namespace Zadatak2.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            Dictionary<Product, Int32> cart = (Dictionary<Product, Int32>)Session["cart"];
            if (cart == null)
                return RedirectToAction("Index", "Authentication");
            
            double totalPrice = 0;
            foreach(var product in cart.Keys)
            {
                totalPrice += (cart[product] * product.price);
            }

            ViewBag.TotalPrice = totalPrice;
            return View(cart);
        }

        public ActionResult Buy()
        {
            Dictionary<Product, Int32> cart = (Dictionary<Product, Int32>)Session["cart"];
            if (cart == null)
                return RedirectToAction("Index", "Authentication");

            Session["cart"] = new Dictionary<Product, Int32>();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Add(string productId)
        {
            Dictionary<Product, Int32> cart = (Dictionary<Product, Int32>)Session["cart"];
            List<Product> products = (List<Product>)HttpContext.Application["products"];
            Product product = products.Find(prod => prod.id.Equals(productId));
            if (cart.ContainsKey(product))
            {
                int quantity = cart[product];
                quantity++;
                cart[product] = quantity;
            }
            else
            {
                cart.Add(product, 1);
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}