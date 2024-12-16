using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WarehouseAsp.Models
{
    public class PaginetedResult<T>
    {
        public List<T> Results { get; set; }
        public string search { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int Pages { get; set; }
    }
}