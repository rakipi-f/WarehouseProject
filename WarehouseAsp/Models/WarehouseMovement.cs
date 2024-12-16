using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WarehouseAsp.Models
{
    public class WarehouseMovement
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int ProductId { get; set; }
        public double Qty { get; set; }

        //public virtual Product Product { get; set; }
    }
}