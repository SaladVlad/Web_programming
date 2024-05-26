using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ShoppingCart
{
    public List<ShoppingCartItem> items { get; set; }
    public double Total
    {
        get
        {
            double retVal = 0;
            foreach (ShoppingCartItem item in items)
            {
                retVal += item.total;
            }
            return retVal;
        }
    }

    public ShoppingCart()
    {
        items = new List<ShoppingCartItem>();
    }

    public void Add(ShoppingCartItem item)
    {
        items.Add(item);
    }

}
