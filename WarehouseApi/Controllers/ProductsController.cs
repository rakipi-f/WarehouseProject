using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
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
        public async Task<IHttpActionResult> Get()
        {

            //ottengo la lista dei prodotti
            var products = await context.Products
                .ToListAsync();
          
            return Ok(mapper.Map<List<ProductDto>>(products));
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
     
            var product = await context.Products
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ProductDto>(product));
        }



        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}