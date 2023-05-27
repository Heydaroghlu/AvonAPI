using Avon.Application.DTOs.ColorDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.ColorValidators
{
    public class ColorCreateDtoValidator : AbstractValidator<ColorCreateDTO>
    {

        public ColorCreateDtoValidator()
        {
            RuleFor(x => x.Discount)
                .NotEmpty().NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .MaximumLength(100);

            RuleFor(x => x.ColorName)
                .NotEmpty().NotNull()
                .MaximumLength(100);
        }
    }
}
