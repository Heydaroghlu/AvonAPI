using Avon.Application.DTOs.ProductDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.ProductValidators
{
    public class ProductEditDtoValidator : AbstractValidator<ProductEditDto>
    {

        public ProductEditDtoValidator()
        {

           RuleFor(x => x.Id)
                .NotEmpty().NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.IsNew).NotNull();

            //RuleFor(x => x.Width)
            //     .NotEmpty().NotNull()
            //     .MaximumLength(50);

            //RuleFor(x => x.Height)
            //    .NotEmpty().NotNull()
            //    .MaximumLength(50);

            //RuleFor(x => x.Weight)
            //    .NotEmpty().NotNull()
            //    .MaximumLength(50);

            //RuleFor(x => x.Lenght)
            //    .NotEmpty().NotNull()
            //    .MaximumLength(50);

            RuleFor(x => x.StockCount)
                .NotEmpty().NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.CostPrice)
                .NotEmpty().NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.SalePrice)
                .NotEmpty().NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.DiscountPrice)
                .NotEmpty().NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Description)
                .NotEmpty().NotNull()
                .MaximumLength(500);

            RuleFor(x => x.inStock).NotNull();

            RuleFor(x => x.PreOrder).NotNull();

            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .MaximumLength(200);

            RuleFor(x => x.ProductSubCategoryIds)
                .NotEmpty().NotNull();
        }

    }
}
