using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QualityBags.Models
{
    /// <summary>
    /// Entity Class for Suppliers
    /// </summary>
    public class Supplier
    {
        //Properties
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Contact Number")]
        public int ContactNumber { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        //Navigation Properties
        public ICollection<Product> Products { get; set; }
    }
}
