using System;
using System.Collections.Generic;

#nullable disable

namespace CoffeeShopManagement.Models
{
    public partial class FoodCategory
    {
        public FoodCategory()
        {
            Foods = new HashSet<Food>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Food> Foods { get; set; }
    }
}
