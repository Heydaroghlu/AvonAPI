using Avon.Application.DTOs.SliderDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.SliderValidators
{
    public class SliderCreateDtoValidator : AbstractValidator<SliderCreateDto>
    {

        public SliderCreateDtoValidator()
        {
            
            RuleFor(x => x.Title2)
                .NotNull().NotEmpty()
                .MaximumLength(50);
         
            RuleFor(x => x.Title1)
                .NotNull().NotEmpty()
                .MaximumLength(50);
        }

    }
}
