using AutoMapper;
using Guide.Application.Data.UnitOfWork;
using Guide.Application.ViewModel.CommandResponses.Product;
using Guide.Core.DomainModels;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Guide.Application.ApplicationEvents.Events.ProductEvents;
using Guide.Common.Interfaces;
using Guide.Core.DomainEvents;
using Guide.Core.DomainEvents.Events.ProductEvents;

namespace Guide.Application.CQRS.Command
{
    public sealed class ProductChangePriceCommand : IRequest<ProductCreateView>
    {
        public ProductChangePriceCommand(int id, decimal productPrice)
        {
            Id = id;
            ProductPrice = productPrice;
        }
        public int Id { get; }

        public decimal ProductPrice { get; }



        //[AuditLog]
        public sealed class ProductChangePriceCommandHandler : IRequestHandler<ProductChangePriceCommand, ProductCreateView>
        {
            private readonly IUnitOfWork<int> _uow;
            private readonly IEventBus _eventPublisher;

            public ProductChangePriceCommandHandler(IUnitOfWork<int> uow, IEventBus eventPublisher)
            {
                _uow = uow;
                _eventPublisher = eventPublisher;
            }

            public Task<ProductCreateView> Handle(ProductChangePriceCommand request, CancellationToken cancellationToken)
            {
                using (_uow)
                {
                    var product = _uow.ProductRepository.Get(request.Id);
                    product.ChangePrice(request.ProductPrice);
                    _uow.GenericRepo.Update(product);
                   

                    if (!_uow.SaveChanges(product))
                        throw new InvalidOperationException();

                    var result = Mapper.Map<ProductCreateView>(product);
                    
                    //product.Raise(new ProductPriceChangedDomainEvent() { LogMessage = "Event from command handler" });
                    //_eventPublisher.Publish(new ProductPriceChangedEvent() { LogMessage = "Event from command handler" });
                    _eventPublisher.Publish(new ProductPriceChangedApplicationEvent() {ProductPrice = product.ProductPrice, ProductCode =  product.Code});
                    return Task.FromResult(result);
                }
            }
        }
    }



}
