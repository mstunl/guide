using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guide.Application.ExceptionHandling
{
    #region Product Exceptions
    public class ProductCodeMustStartWithPLetterException : ValidationException
    {
        public ProductCodeMustStartWithPLetterException(string message) 
            : base(message)
        {
            
        }
    }

    public class ProductStockAmountMustGreaterThanZeroException : ValidationException
    {
        public ProductStockAmountMustGreaterThanZeroException(string message)
            : base(message)
        {

        }
    }
    #endregion

}
