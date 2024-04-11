using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vezbe4
{
    internal class Book
    {
        string name, author;
        double price;

        public Book(string name, string author, double price)
        {
            this.name = name;
            this.author = author;
            this.price = price;
        }

        public string Name { get => name; set => name = value; }
        public string Author { get => author; set => author = value; }
        public double Price { get => price; set => price = value; }

        public override string ToString()
        {
            return "Name: " + Name + " Author: " + Author + " Price: " + Price.ToString();
        }
    }
}
