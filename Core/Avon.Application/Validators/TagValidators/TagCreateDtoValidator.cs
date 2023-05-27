using Avon.Application.DTOs.TagDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.TagValidators
{
    public class TagCreateDtoValidator : AbstractValidator<TagCreateDto>
    {
        public TagCreateDtoValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .MaximumLength(100);
        }
    }
}
