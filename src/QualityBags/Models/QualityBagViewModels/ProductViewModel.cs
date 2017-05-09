using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBags.Models.QualityBagViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
    }
}
