using Avon.Application.DTOs.CategoryDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.CategoryValidators
{
    public class CategoryEditDtoValidator : AbstractValidator<CategoryEditDto>
    {
        public CategoryEditDtoValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty().NotNull()
                    .MaximumLength(50);

            //RuleFor(x => x.DisCount)
            //    .NotEmpty().NotNull()
            //    .GreaterThanOrEqualTo(0);


        }
    }
}
