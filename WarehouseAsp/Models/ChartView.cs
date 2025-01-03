using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WarehouseAsp.Models
{
    public class ChartView
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public List<WarehouseMovement> Movements { get; set; }
        public double GiacenzaTot {  get; set; }
    }

}