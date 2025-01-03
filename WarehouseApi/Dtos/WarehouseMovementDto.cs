using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WarehouseApi.Models;

namespace WarehouseApi.Dtos
{
    public class WarehouseMovementDto
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int ProductId { get; set; }
        public double Qty { get; set; }
        public string ProductName { get; set; }

        //public virtual ProductDto Product { get; set; }
    }
}