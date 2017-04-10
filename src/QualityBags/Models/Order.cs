using System;
using System.Collections.Generic;

namespace QualityBags.Models
{
    /// <summary>
    /// Entity Class for Orders
    /// </summary>
    public class Order
    {
        //Properties
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GST { get; set; }
        public decimal GrandTotal { get; set; }

        //Navigation Properties
        public Customer Customer;
        public ICollection<OrderDetail> OrderDetails;
    }
}
