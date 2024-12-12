﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WarehouseAsp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Images { get; set; }
    }
}