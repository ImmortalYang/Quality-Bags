using System;
using System.Collections.Generic;

namespace QualityBags.Models
{
    /// <summary>
    /// Entity Class for Products
    /// </summary>
    public class Product
    {
        //Properties
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        //Navigation Properties
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
