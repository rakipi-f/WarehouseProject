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

                //.ForMember(d => d.ProductId, m => m.MapFrom(s => s.Product.Id))
               // .ForMember(d => d.Product, m => m.Ignore());

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}