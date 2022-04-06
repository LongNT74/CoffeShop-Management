using System;
using System.Collections.Generic;

#nullable disable

namespace CoffeeShopManagement.Models
{
    public partial class Account
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PassWord { get; set; }
        public int Type { get; set; }
    }
}
