﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dm106CarlosDrury.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int qtd { get; set; }

        // Foreign Key
        public int ProductId { get; set; }
        // Navigation property
        public virtual Product Product { get; set; }
        public int OrderId { get; set; }
    }
}