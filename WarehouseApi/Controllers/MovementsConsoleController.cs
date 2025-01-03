﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using WarehouseApi.Dtos;
using WarehouseApi.Models;

namespace WarehouseApi.Controllers
{
    [RoutePrefix("api/WarehouseMovements")]
    public class MovementsConsoleController : ApiController
    {
        private WarehouseEntities context;
        private MapperConfiguration mc;
        private Mapper mapper;

        public MovementsConsoleController()
        {
            context = new WarehouseEntities();
            mc = new MapperConfiguration(cfg => cfg.AddProfile<DtoMappingProfile>());
            mapper = new Mapper(mc);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get( int id )
        {


            var warehouseMovement = await context.WarehouseMovements
                .Where(a => a.ProductId == id)
                .OrderByDescending(wm => wm.Date)
                .ToListAsync();



            if (warehouseMovement == null || !warehouseMovement.Any())
            {
                return NotFound();
            }
            

            // calcolo la giacenza
            var giacenza = warehouseMovement.Sum(a => a.Qty);

            var FirstMovements = warehouseMovement
                //.Take(5)
                //lo tolgo per rendere l'api utilizzabile per le statistiche
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
