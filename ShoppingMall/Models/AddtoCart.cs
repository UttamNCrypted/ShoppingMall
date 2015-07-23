using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingMall.Models
{
    public class AddtoCart
    {
        public AddtoCart()
        {
            
        }

        //public List<Product> Products { get; set; }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Qty { get; set; }
        public decimal? Price { get; set; }
        public string Size { get; set; }
        public decimal? Discount { get; set; }
        public string ProductImage { get; set; }
        public int? MaxProduct { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
    }
}