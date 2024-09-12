using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeShop.Components.Models
{
    public class Menu
    {

   
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

