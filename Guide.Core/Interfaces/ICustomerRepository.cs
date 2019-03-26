using Guide.Common.Interfaces;
using Guide.Core.DomainModels;

namespace Guide.Core.Interfaces
{
    public interface ICustomerRepository :  IRepository<Customer, int>
    {
    }
}
