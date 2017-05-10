using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBags.Models
{
    public class CartItem
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// ID of the shopping cart
        /// </summary>
        public string CartID { get; set; }
        public int Count { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public Product Product { get; set; }
    }
}
