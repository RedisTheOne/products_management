using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsManagment.models
{
    public class Product
    {
        public int Id, price;
        public string title;
        public Product(int Id, int price, string title)
        {
            this.Id = Id;
            this.price = price;
            this.title = title;
        }
        public Product()
        {

        }

    }
}
