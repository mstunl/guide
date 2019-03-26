using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guide.Core.DomainModels;
using Guide.Core.Interfaces;

namespace Guide.Application.Data.Repositories
{
    public class ProductRepository : RepositoryBase<GuideContext>, IProductRepository
    {
        public ProductRepository(GuideContext context) 
            : base(context)
        {
        }

        #region Generic Methods
        public Product Get(int id)
        {
           return Context.Products.SingleOrDefault(p => p.Id == id);
        }

        public void Insert(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        } 
        #endregion

        #region Custom Methods

        public IEnumerable<Product> GetAllProducts()
        {
            return Context.Products.ToList();

        } 
        #endregion
    }
}
