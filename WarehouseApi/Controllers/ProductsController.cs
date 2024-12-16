using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using WarehouseApi.Dtos;
using WarehouseApi.Models;

namespace WarehouseApi.Controllers
{
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        // GET: Products
         
        private WarehouseEntities context;
        private MapperConfiguration mc;
        private Mapper mapper;

        public ProductsController()
        {
            context = new WarehouseEntities();
            mc = new MapperConfiguration(cfg => cfg.AddProfile<DtoMappingProfile>());
            mapper = new Mapper(mc);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int page = 1, int pagesize =10, string search="")
        {
        
            //ottengo la lista dei prodotti
            var query = context.Products
                .ProjectTo<ProductDto>(mc);

            int number; 
            bool isNumber = int.TryParse(search, out number);
            IQueryable<ProductDto> filteredQuery;



            //qui controllo se search è un numero allora filtro per id altrimenti filtro per titolo e descrizione
            if (string.IsNullOrEmpty(search)) 
                filteredQuery = query;
            else if (isNumber) 
                filteredQuery = query.Where(w => w.Id == number);
            else
                filteredQuery = query.Where(w => w.Title.Contains(search) || w.Description.Contains(search));

            int totalRecordCount = await filteredQuery
                .CountAsync();

            var results = await filteredQuery
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return Ok(new PaginetedResultDto<ProductDto>
            {
                Results = results,
                Search = search,
                Page = page,
                PageSize = pagesize,
                Pages = (int)Math.Ceiling((double)totalRecordCount / pagesize)

            });


        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(int id, int page=1, int pagesize=10, string search="")
        {
     
            var product = await context.Products
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(new PaginetedResultDto<ProductDto>
            {
                Results = new List<ProductDto> { mapper.Map<ProductDto>(product) },
                Search = search,
                PageSize = pagesize,
                Page = page,

            });
        }
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}