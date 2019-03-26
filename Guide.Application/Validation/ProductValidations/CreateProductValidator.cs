using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Guide.Application.CQRS.Command;
using Guide.Application.ExceptionHandling;

namespace Guide.Application.Validation.ProductValidations
{
    public class CreateProductValidator : AbstractValidator<ProductCreateCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p).NotNull().OnAnyFailure(x =>
            {
                throw new ArgumentNullException(nameof(x));
            });

            RuleFor(p => p.Name).MinimumLength(10).WithMessage("Ürün adı en az 10 karakter olmalıdır!");

            RuleFor(p => p.StockAmount)
                .GreaterThan(0).OnAnyFailure(x =>
                {
                    throw new ProductStockAmountMustGreaterThanZeroException("Stok miktarı 0'dan büyük olmalıdır!");
                });

            RuleFor(p => p.Code).NotEmpty().WithMessage("Ürün adı en az 10 karakter olmalıdır!");


        }
    }
}
