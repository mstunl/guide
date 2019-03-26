using AutoMapper;
using Guide.Application.Data;
using Guide.Application.Utils.Filtering;
using Guide.Application.ViewModel.QueryResponses.Product;
using Guide.Core.DomainModels.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Guide.Application.CQRS.Query
{
    public sealed class ProductListQuery : IRequest<List<ProductView>> //BaseRequest<List<ProductView>, Product>
    {
        public ProductListQuery(string name, int amount, string code)
        {
            Name = name;
            StockAmount = amount;
            Code = code;
        }

        public string Name { get; }
        public int StockAmount { get; }
        public string Code { get; set; }

        internal class ProductListQueryHandler : IRequestHandler<ProductListQuery, List<ProductView>>
        {
            private readonly GuideContext _guideContext;


            public ProductListQueryHandler(GuideContext guideContext)
            {
                _guideContext = guideContext;
            }



            public Task<List<ProductView>> Handle(ProductListQuery request, CancellationToken cancellationToken)
            {

                var products = _guideContext.ProductListDtos.AsQueryable();
                
                var filters = new List<QueryFilter>()
                    .GrowFilter("Name", Op.Equals, request.Name)
                    .GrowFilter("StockAmount", Op.GreaterThan, request.StockAmount);


                var func = ExpressionBuilder.BuildFilter<ProductListDto>(filters).Compile();
                var filteredResult = products.AsEnumerable().Where(func).ToList();

                var mappingResult = Mapper.Map<List<ProductView>>(filteredResult);

                return Task.FromResult(mappingResult);
            }
        }

    }

}
