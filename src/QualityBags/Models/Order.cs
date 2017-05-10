using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Grand Total")]
        public decimal GrandTotal { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        
        [Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode, ErrorMessage = "Please enter a valid postal code.")]
        public string PostalCode { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }

        //Navigation Properties
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
