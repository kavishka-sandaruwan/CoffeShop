using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeShop.Components.Models
{
    public class CoffeeOrder
    {
        public string Id {  get; set; }
        public string CustomerName { get; set; }
        public string CoffeeType { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public bool HasExtraShot { get; set; }
        public bool HasWhippedCream { get; set; }
        public decimal Price { get; set; }
    }
}
