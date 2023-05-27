using Avon.Application.DTOs.BrandDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.BrandValidators
{
    public class BrandCreateDtoValidator : AbstractValidator<BrandCreateDto>
    {
        public BrandCreateDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty()
                .MaximumLength(100);
        }
    }
}
