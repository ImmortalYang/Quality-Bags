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
        public string ApplicationUserId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GST { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime OrderDate { get; set; }

        //Navigation Properties
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
