using System;
using System.Collections;

namespace Bookstore.Interface
{
    public class Book 
       
    {
        private string title;
        private string author;
        private decimal price;
        private int inStock;

        public Book()
        {
        }

        public string Title { get => title; set => title = value; }
        public string Author { get => author; set => author = value; }
        public decimal Price { get => price; set => price = value; }
        public int InStock { get => inStock; set => inStock = value; }

    }




}
