using System;
using System.Collections.Generic;

namespace QualityBags.Models
{
    /// <summary>
    /// Entity Class for Suppliers
    /// </summary>
    public class Supplier
    {
        //Properties
        public int ID { get; set; }
        public string SupplierName { get; set; }
        public int ContactNumber { get; set; }
        public string EmailAddress { get; set; }

        //Navigation Properties
        public ICollection<Product> Products { get; set; }
    }
}
