using Avon.Application.DTOs.SizeDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.SizeValidators
{
    public class SizeCreateDtoValidator : AbstractValidator<SizeCreateDTO>
    {

        public SizeCreateDtoValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .MaximumLength(100);

            RuleFor(x => x.SizeName)
                .NotEmpty().NotNull()
                .MaximumLength(100);

            RuleFor(x => x.Discount)
                //todo int not empty yigisdir
                .NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Width)
                .NotEmpty().NotNull();

            RuleFor(x => x.Height)
                .NotEmpty().NotNull();
        }
    }
}
