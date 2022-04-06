using System;
using System.Collections.Generic;

#nullable disable

namespace CoffeeShopManagement.Models
{
    public partial class Food
    {
        public Food()
        {
            BillInfos = new HashSet<BillInfo>();
        }

        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int IdCategory { get; set; }
        public double Price { get; set; }

        public virtual FoodCategory IdCategoryNavigation { get; set; }
        public virtual ICollection<BillInfo> BillInfos { get; set; }
    }
}
