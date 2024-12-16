using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WarehouseApi.Dtos
{
    public class PaginetedResultDto<T>
    {
        public List<T> Results { get; set; }
        public string Search = "";
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Pages{ get; set; }
    }
}