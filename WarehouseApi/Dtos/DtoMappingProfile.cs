using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WarehouseApi.Models;

namespace WarehouseApi.Dtos
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<WarehouseMovement, WarehouseMovementDto>();
            CreateMap<WarehouseMovementDto, WarehouseMovement>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}