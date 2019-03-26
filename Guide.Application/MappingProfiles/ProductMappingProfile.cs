using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Guide.Application.ViewModel;
using Guide.Application.ViewModel.CommandResponses.Product;
using Guide.Application.ViewModel.QueryResponses.Product;
using Guide.Core.DomainModels;
using Guide.Core.DomainModels.Dtos;

namespace Guide.Application.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductView>();
            CreateMap<Product, ProductCreateView>();
            CreateMap<ProductListDto, ProductView>();
        }
    }
}
