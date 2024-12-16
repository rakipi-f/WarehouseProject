using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using AutoMapper;
using WarehouseApi.Dtos;
using WarehouseApi.Models;

namespace WarehouseApi.Controllers
{
    [RoutePrefix("api/Stocks")]
    public class StocksController : ApiController
    {
        private WarehouseEntities context;
        private MapperConfiguration mc;
        private Mapper mapper;

        public StocksController()
        {
            context = new WarehouseEntities();
            mc = new MapperConfiguration(cfg => cfg.AddProfile<DtoMappingProfile>());
            mapper = new Mapper(mc);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {

            //ottengo gli ultimi 5 movimenti come lista
            var warehouseMovement = await context.WarehouseMovements
                .OrderByDescending(wm => wm.Date)
                .ToListAsync();
            // controllo se l'id è valido o non esiste


            if (warehouseMovement == null || !warehouseMovement.Any())
            {
                return NotFound();
            }


            // calcolo la giacenza
            var giacenza = warehouseMovement.Sum(a => a.Qty);

            var FirstMovements = warehouseMovement
                .Take(5)
                .ToList();


            // controlli sulla giacenza

            //restituisco sia la lista che la giacenza in un unico oggetto
            var viewModel = new ViewModelConsole
            {
                Movements = mapper.Map<List<WarehouseMovementDto>>(FirstMovements),
                Giacenza = giacenza
            };
            return Ok(viewModel);
        }




        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }


    }
}