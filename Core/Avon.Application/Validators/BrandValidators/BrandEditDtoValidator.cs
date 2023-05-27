using Avon.Application.DTOs.BrandDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.BrandValidators
{
    public class BrandEditDtoValidator : AbstractValidator<BrandEditDto>
    {
        public BrandEditDtoValidator()
        {
            RuleFor(x => x.Name)
             .NotNull().NotEmpty()
             .MaximumLength(100);
        }
    }
}
