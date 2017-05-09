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
        public int ApplicationUserId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GST { get; set; }
        public decimal GrandTotal { get; set; }

        //Navigation Properties
        public ApplicationUser AppUser { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
