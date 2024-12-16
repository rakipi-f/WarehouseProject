using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WarehouseAsp.Models
{

        public class ViewModelConsole
        {
            public List<WarehouseMovement> Movements { get; set; }
            public double Giacenza { get; set; }
        }
    
}