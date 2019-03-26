using AutoMapper;
using Guide.Application.Data.UnitOfWork;
using Guide.Application.ViewModel.CommandResponses.Product;
using Guide.Core.DomainModels;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Guide.Application.CQRS.Command
{
    public sealed class ProductCreateCommand : IRequest<ProductCreateView>
    {
        public ProductCreateCommand(string name, string code, int stockAmount)
        {
            Name = name;
            Code = code;
            StockAmount = stockAmount;
        }

        public string Name { get; }
        public string Code { get; }
        public int StockAmount { get; }

        
    //[AuditLog]
    public sealed class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, ProductCreateView>
    {
        private readonly IUnitOfWork<int> _uow;


        public ProductCreateCommandHandler(IUnitOfWork<int> uow)
        {
            _uow = uow;
        }

        public Task<ProductCreateView> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            using (_uow)
            {
                var product = Product.CreateProduct(request.Name, request.Code, request.StockAmount);
                _uow.GenericRepo.Insert(product);

                if (!_uow.SaveChanges(product))
                    throw new InvalidOperationException();

                var result = Mapper.Map<ProductCreateView>(product);

                return Task.FromResult(result);
            }
        }
    }
    }



}
