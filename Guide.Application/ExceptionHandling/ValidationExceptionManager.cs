using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guide.Application.ExceptionHandling
{
    public class ValidationExceptionManager
    {
        public static string GetCustomValidationExceptionMessage(Exception ex)
        {
            string content = "";

            #region Product

            if (ex is ProductCodeMustStartWithPLetterException)
            {
                //content = "Ürün kodu 'P' harfi ile başlamalıdır!"; // Localization kullanılabilir.
                content = ex.Message;
            }
            if (ex is ProductStockAmountMustGreaterThanZeroException)
            {
                //content = "Ürün kodu 'P' harfi ile başlamalıdır!"; // Localization kullanılabilir.
                content = ex.Message;
            }
            #endregion
            return content;
        }
    }
}
