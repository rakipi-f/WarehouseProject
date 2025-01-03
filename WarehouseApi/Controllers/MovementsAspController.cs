﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using WarehouseApi.Dtos;
using WarehouseApi.Models;

namespace WarehouseApi.Controllers
{
    [RoutePrefix("api/Movements")]
    public class MovementsAspController : ApiController
    {
        // GET: MovementsAsp
        private WarehouseEntities context;
        private MapperConfiguration mc;
        private Mapper mapper;

        public MovementsAspController()
        {
            context = new WarehouseEntities();
            mc = new MapperConfiguration(cfg => cfg.AddProfile<DtoMappingProfile>());
            mapper = new Mapper(mc);
        }

        //get list
        [HttpGet]
        [Route("")]

        public async Task<IHttpActionResult> Get(int page = 1, int pagesize = 10, string search = "")
        {
            var query = from wm in context.WarehouseMovements
                        join p in context.Products on wm.ProductId equals p.Id
                        select new WarehouseMovementDto
                        {
                            Id = wm.Id,
                            ProductId = wm.ProductId,
                            ProductName = p.Title,
                            Date = wm.Date,
                            Qty = wm.Qty
                        };

            int number;
            bool isNumber = int.TryParse(search, out number);
            IQueryable<WarehouseMovementDto> filteredQuery;

            if (string.IsNullOrEmpty(search))
                filteredQuery = query;
            else if (isNumber)
                filteredQuery = query.Where(w => w.ProductId == number);
            else
                filteredQuery = query.Where(w => w.ProductName.Contains(search));

            int TotalRecordCount = await filteredQuery.CountAsync();

            var warehouseMovement = await filteredQuery
                .OrderByDescending(wm => wm.Date)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            var results = new List<WarehouseMovementDto>();
            if (warehouseMovement != null || warehouseMovement.Any())
            {
                results = warehouseMovement; // Già mappato nella query
            }

            return Ok(new PaginetedResultDto<WarehouseMovementDto>
            {
                Results = results,
                Search = search,
                PageSize = pagesize,
                Page = page,
                Pages = (int)Math.Ceiling((double)TotalRecordCount / pagesize)
            });
        }


        //get 1 movement
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var warehouseMovement = await context.WarehouseMovements
                .Where(a => a.Id == id)
                .ProjectTo<WarehouseMovementDto>(mc)
                .FirstOrDefaultAsync();
            return Ok(warehouseMovement);
        }

        //create
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(WarehouseMovementDto warehouseMovementDto)
        {
            var warehouseMovement = mapper.Map<WarehouseMovement>(warehouseMovementDto);
            var product = await context.Products.FirstOrDefaultAsync(q => q.Id == warehouseMovement.ProductId);
            if (product == null)
                return NotFound();
            context.WarehouseMovements.Add(warehouseMovement);
            await context.SaveChangesAsync();
            return Ok();

        }

        //edit
        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Put(int id, WarehouseMovementDto warehouseMovementDto)
        {
            var warehouseMovement = await context.WarehouseMovements.FirstOrDefaultAsync(q => q.Id == id);
            if (warehouseMovement == null)
                return NotFound();
            warehouseMovement.Date = warehouseMovementDto.Date;
            warehouseMovement.ProductId = warehouseMovementDto.ProductId;
            warehouseMovement.Qty = warehouseMovementDto.Qty;
            await context.SaveChangesAsync();
            return Ok();
        }



        //delete
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var warehouseMovement = await context.WarehouseMovements.FirstOrDefaultAsync(q => q.Id == id);
            if (warehouseMovement == null)
                return NotFound();
            context.WarehouseMovements.Remove(warehouseMovement);
            await context.SaveChangesAsync();
            return Ok();
        }




        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }


    }
}
