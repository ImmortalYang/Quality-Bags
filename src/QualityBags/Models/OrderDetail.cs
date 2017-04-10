﻿using System;
using System.Collections.Generic;

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
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }

        //Navigation Properties
        public Product Product;
        public Order Order;
    }
}