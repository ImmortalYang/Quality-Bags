using System;
using System.Collections.Generic;

namespace QualityBags.Models
{
    /// <summary>
    /// Entity Class for Categories
    /// </summary>
    public class Category
    {
        //Properties
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        //Navigation Properties
        public ICollection<Product> Products { get; set; }
    }
}
