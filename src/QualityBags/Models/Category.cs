using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QualityBags.Models
{
    /// <summary>
    /// Entity Class for Categories
    /// </summary>
    public class Category
    {
        //Properties
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string CategoryName { get; set; }
        public string Description { get; set; }

        //Navigation Properties
        public ICollection<Product> Products { get; set; }
    }
}
