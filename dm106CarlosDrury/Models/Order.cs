﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dm106CarlosDrury.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string emailUser { get; set; }

        public DateTime orderDate { get; set; }

        public DateTime? deliveryDate { get; set; }

        public string status { get; set; }

        public decimal totalPrice { get; set; }

        public decimal totalWeigth { get; set; }

        public decimal shipmentPrice { get; set; }

        public virtual ICollection<OrderItem> OrderItems
        {
            get; set;
        }
    }
}