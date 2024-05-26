using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ShoppingCartItem
{
    public Product product { get; set; }
    public int count { get; set; }
    public double total
    {
        get
        {
            return count * product.price;
        }
    }

    public ShoppingCartItem() { }
    public ShoppingCartItem(Product product, int count) : this()
    {
        this.product = product;
        this.count = count;
    }

}
