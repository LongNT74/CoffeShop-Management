using System;
using System.Collections.Generic;

#nullable disable

namespace CoffeeShopManagement.Models
{
    public partial class TableFood
    {
        public TableFood()
        {
            Bills = new HashSet<Bill>();
        }

        public int? TableId { get; set; }
        public string TableName { get; set; }
        public string TableStatus { get; set; }

        public virtual ICollection<Bill> Bills { get; set; }
    }
}
