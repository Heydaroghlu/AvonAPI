using Avon.Application.DTOs.CommentDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.CommentValidators
{
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {

        public CommentCreateDtoValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().NotNull()
                .MaximumLength(500);

            RuleFor(x => x.Name)
                .NotEmpty().NotNull();

            RuleFor(x => x.Star)
                .NotEmpty().NotNull()
                .InclusiveBetween(1, 5);
        }
    }
}
