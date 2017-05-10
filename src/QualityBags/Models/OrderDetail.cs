using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QualityBags.Models
{
    /// <summary>
    /// Entity Class for Order Details
    /// </summary>
    public class OrderDetail
    {
        //Properties
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        //Navigation Properties
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
