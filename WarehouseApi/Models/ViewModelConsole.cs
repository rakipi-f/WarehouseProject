using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WarehouseApi.Dtos;

namespace WarehouseApi.Models
{
    public class ViewModelConsole
    {
        public List<WarehouseMovementDto> Movements { get; set; }
        public double Giacenza { get; set; }
    }
}