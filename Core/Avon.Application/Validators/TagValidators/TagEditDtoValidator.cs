using Avon.Application.DTOs.TagDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.TagValidators
{
    public class TagEditDtoValidator : AbstractValidator<TagEditDto>
    {
        public TagEditDtoValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .MaximumLength(100);
        }
    }
}
