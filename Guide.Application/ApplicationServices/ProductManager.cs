using AutoMapper;
using Guide.Application.CQRS.Command;
using Guide.Application.CQRS.Query;
using Guide.Application.ViewModel.CommandResponses.Product;
using Guide.Application.ViewModel.QueryResponses.Product;
using Guide.Core.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Guide.Application.ApplicationServices
{

    public class ProductManager
    {
        private readonly IProductRepository _repo;
        private readonly IMediator _mediator;

        public ProductManager(IProductRepository repo, IMediator mediator)
        {
            _repo = repo;
            _mediator = mediator;
        }

        public IList<ProductView> GetProductsByStockAmount(ProductListQuery query)
        {
            var result = _mediator.Send(query).Result;
            return Mapper.Map<IList<ProductView>>(result);
        }

        public ProductView GetProductById(int id)
        {
            var product = _repo.Get(id);
            return Mapper.Map<ProductView>(product);
        }
        public ProductCreateView CreateProduct(ProductCreateCommand command)
        {
            var result = _mediator.Send(command).Result;
            return result;
        }

        public ProductCreateView ChangeProductPrice(ProductChangePriceCommand command)
        {
            var result = _mediator.Send(command).Result;
            
            return result;
        }
    }
}
