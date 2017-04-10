using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBags.Models
{
    /// <summary>
    /// Entity Class for Customers
    /// </summary>
    public class Customer
    {
        //Properties
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public int ContactNumber { get; set; }
        public string EmailAddress { get; set; }

        //Navigation Properties
        public ICollection<Order> Orders;
    }
}
