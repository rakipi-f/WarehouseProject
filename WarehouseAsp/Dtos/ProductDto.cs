﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WarehouseAsp.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
    }
}